using api.core.data.entities;
using api.core.Data.Entities;
using api.core.Data.Exceptions;
using api.core.Data.requests;
using api.core.Data.Responses;
using api.core.repositories.abstractions;
using api.core.services.abstractions;
using api.files.Services.Abstractions;

using Azure.Core;

using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace api.core.Services;

public class EventService(
    IEventRepository evntRepo,
    ITagRepository tagRepo,
    IOrganizerRepository orgRepo,
    IModeratorRepository moderatorRepo,
    IFileShareService fileShareService,
    IConfiguration configuration) : IEventService
{
    private const double IMAGE_RATIO_SIZE_ACCEPTANCE = 2.0; // width/height ratio
    private const double TOLERANCE_ACCEPTABILITY = 0.001;

    public IEnumerable<EventResponseDTO> GetEvents(
        DateTime? startDate,
        DateTime? endDate,
        IEnumerable<string>? activityAreas,
        IEnumerable<Guid>? tags,
        Guid? organizerId,
        State state,
        bool ignorePublicationDate = false)
    {
        var events = evntRepo.GetAll();

        return events.Where(e =>
         e.Publication.DeletedAt == null &&
         (ignorePublicationDate || e.Publication.PublicationDate <= DateTime.UtcNow) &&
         (startDate == null || e.EventEndDate >= startDate) &&
         (endDate == null || e.EventStartDate <= endDate) &&
         (state.HasFlag(e.Publication.State)) &&
         (organizerId == null || e.Publication.OrganizerId == organizerId) &&
         (tags.IsNullOrEmpty() || e.Publication.Tags.Any(t => tags.Any(tt => t.Id == tt))) &&
         (activityAreas.IsNullOrEmpty() || activityAreas.Any(aa => aa == e.Publication.Organizer.ActivityArea)))
            .OrderBy(e => e.EventStartDate)
            .Select(EventResponseDTO.Map);
    }

    public EventResponseDTO GetEvent(Guid id)
    {
        var evnt = evntRepo.Get(id);
        NotFoundException<Event>.ThrowIfNull(evnt);

        return EventResponseDTO.Map(evnt);
    }

    public EventResponseDTO AddEvent(Guid userId, EventCreationRequestDTO request)
    {
        var organizer = orgRepo.Get(userId) ?? throw new UnauthorizedException();

        var tags = tagRepo.GetAll()
            .Where(t => request.Tags.Contains(t.Id))
            ?? Enumerable.Empty<Tag>();

        var id = Guid.NewGuid();
        var cdnUrl = configuration.GetValue<string>("CDN_URL");
        var completePath = $"{cdnUrl}/{id}/{request.Image.FileName}";
        var uri = new Uri(completePath);

        HandleImageSaving(id, request.Image);

        var inserted = evntRepo.Add(new Event
        {
            Id = id,
            EventStartDate = request.EventStartDate,
            EventEndDate = request.EventEndDate,
            Publication = new Publication
            {
                Id = id,
                Title = request.Title,
                Content = request.Content,
                ImageUrl = uri.ToString(),
                ImageAltText = request.ImageAltText,
                State = State.OnHold,
                PublicationDate = request.PublicationDate,
                Tags = tags.ToList(),
                Organizer = organizer,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
        });

        return EventResponseDTO.Map(inserted);
    }

    public bool DeleteEvent(Guid userId, Guid eventId)
    {
        var eventToDelete = evntRepo.Get(eventId);
        NotFoundException<Event>.ThrowIfNull(eventToDelete);

        if (!CanPerformAction(userId, eventToDelete!))
            throw new UnauthorizedException();

        return evntRepo.Delete(eventToDelete!);
    }


    public bool UpdateEvent(Guid userId, Guid eventId, EventUpdateRequestDTO request)
    {
        var organizer = orgRepo.Get(userId) ?? throw new UnauthorizedException();
        var evnt = evntRepo.Get(eventId);

        if (evnt!.Publication.Organizer != null && evnt.Publication.Organizer.Id != userId)
            throw new UnauthorizedException();

        var tags = tagRepo.GetAll()
            .Where(t => request.Tags.Contains(t.Id))
            ?? Enumerable.Empty<Tag>();

        Uri uri = new Uri(evnt.Publication.ImageUrl);

        if (request.Image != null)
        {
            var cdnUrl = configuration.GetValue<string>("CDN_URL");
            var completePath = $"{cdnUrl}/{evnt.Id}/{request.Image?.FileName}";
            uri = new Uri(completePath);

            HandleImageSaving(eventId, request.Image);
        }

        return evntRepo.Update(eventId, new Event
        {
            Id = eventId,
            EventStartDate = request.EventStartDate,
            EventEndDate = request.EventEndDate,
            Publication = new ()
            {
                Id = eventId,
                Title = request.Title,
                Content = request.Content,
                State = State.OnHold,
                ImageUrl = uri.ToString(),
                PublicationDate = request.PublicationDate,
                ImageAltText = request.ImageAltText,
                Tags = tags.ToList(),
                Organizer = organizer,
                OrganizerId = organizer.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
        });
    }

    public bool UpdateEventState(Guid userId, Guid eventId, State state)
    {
        var moderator = moderatorRepo.Get(userId) ?? throw new UnauthorizedException();
        var evnt = evntRepo.Get(eventId);

        if (evnt!.Publication.ModeratorId == null)
        {
            evnt.Publication.ModeratorId = moderator.Id;
        }

        evnt.Publication.State = state;
        return evntRepo.Update(eventId, evnt);
    }
    private bool CanPerformAction(Guid userId, Event evnt)
    {
        return (evnt!.Publication.Moderator != null && evnt.Publication.Moderator.Id == userId) ||
            (evnt!.Publication.Organizer != null && evnt.Publication.Organizer.Id == userId);
    }

    public int PublishedIfApprovedPassedDue()
    {
        var eventsToUpdate = evntRepo.GetAll()
            .Where(x =>
                x.Publication.PublicationDate < DateTime.UtcNow &&
                x.Publication.DeletedAt == null &&
                x.Publication.State == State.Approved).ToList();

        foreach (var evnt in eventsToUpdate)
        {
            evnt.Publication.State = State.Published;
            evntRepo.Update(evnt.Id, evnt);
        }

        return eventsToUpdate.Count;
    }

    private void HandleImageSaving(Guid eventId, IFormFile imageFile)
    {
        byte[] imageBytes = [];
        try
        {
            using var image = Image.Load(imageFile.OpenReadStream());
            int width = image.Size.Width;
            int height = image.Size.Height;

            if (Math.Abs((width / height) - IMAGE_RATIO_SIZE_ACCEPTANCE) > TOLERANCE_ACCEPTABILITY)
                throw new BadParameterException<Event>(nameof(image), "Invalid image aspect ratio");
            
            image.Mutate(c => c.Resize(400, 200));
            using var outputStream = new MemoryStream();
            image.SaveAsWebp(outputStream);
            outputStream.Position = 0;

            fileShareService.FileUpload(eventId.ToString(), imageFile.FileName, outputStream);
        }
        catch (Exception e)
        {
            throw new BadParameterException<Event>(nameof(imageFile), $"Invalid image metadata: {e.Message}");
        }
    }
}
