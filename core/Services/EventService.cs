using api.core.data.entities;
using api.core.Data.Enums;
using api.core.Data.Exceptions;
using api.core.Data.requests;
using api.core.Data.Responses;
using api.core.repositories.abstractions;
using api.core.services.abstractions;
using api.emails;
using api.emails.Models;
using api.emails.Services.Abstractions;
using api.files.Services.Abstractions;
using api.core.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace api.core.Services;

public class EventService(
    IConfiguration config,
    IEventRepository evntRepo,
    ITagService tagService,
    IOrganizerRepository orgRepo,
    IModeratorRepository moderatorRepo,
    IFileShareService fileShareService,
    IEmailService emailService,
    IImageService imageService) : IEventService
{
    private const int MAX_TITLE_LENGTH = 15;

    public IEnumerable<EventResponseDTO> GetEvents(
        DateTime? startDate,
        DateTime? endDate,
        IEnumerable<Guid>? activityAreas,
        IEnumerable<Guid>? tags,
        Guid? organizerId,
        string? title,
        State state,
        string orderBy = "EventStartDate",
        bool desc = false,
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
         (title == null || (e.Publication.Title != null && e.Publication.Title.ToLower().Contains(title.ToLower()))) &&
         (tags.IsNullOrEmpty() || e.Publication.Tags.Any(t => tags!.Any(tt => t.Id == tt))) &&
         (activityAreas.IsNullOrEmpty() || activityAreas!.Any(aa => aa == e.Publication.Organizer.ActivityAreaId)))
            .AsQueryable()
            .OrderBy(orderBy, desc)
            .Select(EventResponseDTO.Map);
    }

    public EventResponseDTO GetEvent(Guid id)
    {
        var evnt = evntRepo.Get(id);
        NotFoundException<Event>.ThrowIfNull(evnt);

        return EventResponseDTO.Map(evnt!);
    }

    public EventResponseDTO AddEvent(Guid userId, EventCreationRequestDTO request)
    {
        var organizer = orgRepo.Get(userId) ?? throw new UnauthorizedException();

        if (request.Tags.Count > 5)
            throw new BadParameterException<Event>(nameof(request.Tags), "Too many tags");

        var id = Guid.NewGuid();
        string uri = "";
        if (request.Image != null)
        {
            uri = fileShareService.FileGetDownloadUri($"{id}/{request.Image.FileName}").ToString();
            imageService.EnsureImageSizeAndStore(id.ToString(), request.Image, null, ImageType.Publication);
        }
        else if (request.ImageUrl != null && request.ImageUrl.StartsWith(config.GetValue<string>("CDN_URL")!))
            uri = request.ImageUrl;
        else
            throw new BadParameterException<EventCreationRequestDTO>("image", "Nor image nor imageUrl was provided.");

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
                Tags = tagService.GetAssociatedTags(request.Tags),
                Organizer = organizer,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
        });

        return EventResponseDTO.Map(inserted);
    }

    public EventResponseDTO AddDraftEvent(Guid userId, DraftEventRequestDTO request)
    {
        var organizer = orgRepo.Get(userId) ?? throw new UnauthorizedException();

        if (request.Tags.Count > 5)
            throw new BadParameterException<Event>(nameof(request.Tags), "Too many tags");

        var id = Guid.NewGuid();
        string uri = "";

        if (request.Image != null)
        {
            uri = fileShareService.FileGetDownloadUri($"{id}/{request.Image.FileName}").ToString();
            imageService.EnsureImageSizeAndStore(id.ToString(), request.Image, null, ImageType.Publication);
        }
        else if (request.ImageUrl != null && request.ImageUrl.StartsWith(config.GetValue<string>("CDN_URL")!))
            uri = request.ImageUrl;
        else
            throw new BadParameterException<DraftEventRequestDTO>("image", "No image nor imageUrl was provided.");


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
                ImageUrl = uri?.ToString(),
                ImageAltText = request.ImageAltText,
                State = State.Draft,
                PublicationDate = request.PublicationDate,
                Tags = tagService.GetAssociatedTags(request.Tags),
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
        _ = orgRepo.Get(userId) 
            ?? throw new UnauthorizedException();

        if (request.Tags.Count > 5)
            throw new BadParameterException<Event>(nameof(request.Tags), "Too many tags");

        var evnt = evntRepo.Get(eventId);
        NotFoundException<Event>.ThrowIfNull(evnt);
        NotFoundException<Publication>.ThrowIfNull(evnt!.Publication);

        if (evnt!.Publication.Organizer != null && evnt.Publication.Organizer.Id != userId)
            throw new UnauthorizedException();

        string imageUrl = evnt!.Publication!.ImageUrl ?? "";

        if (request.Image != null)
        {
            imageUrl = fileShareService.FileGetDownloadUri($"{evnt.Id}/{request.Image?.FileName}").ToString();
            imageService.EnsureImageSizeAndStore(eventId.ToString(), request.Image!, null, ImageType.Publication);
        }

        evntRepo.ResetTags(eventId);

        evnt.EventStartDate = request.EventStartDate;
        evnt.EventEndDate = request.EventEndDate;
        evnt.Publication.Title = request.Title;
        evnt.Publication.Content = request.Content;
        evnt.Publication.State = State.OnHold;
        evnt.Publication.ImageUrl = imageUrl;
        evnt.Publication.PublicationDate = request.PublicationDate;
        evnt.Publication.ImageAltText = request.ImageAltText;
        evnt.Publication.Tags = tagService.GetAssociatedTags(request.Tags);
        evnt.Publication.UpdatedAt = DateTime.UtcNow;

        return evntRepo.Update(eventId, evnt);
    }


    public bool UpdateEventState(Guid userId, Guid eventId, State state, string? reason)
    {
        var moderator = moderatorRepo.Get(userId) ?? throw new UnauthorizedException();
        var evnt = evntRepo.Get(eventId);

        if (evnt!.Publication.ModeratorId == null)
        {
            evnt.Publication.ModeratorId = moderator.Id;
        }
        if (state == State.Draft)
            return false;

        // If the event is approved and the publication date is in the past, we publish it straight away
        if (state == State.Approved && evnt.Publication.PublicationDate < DateTime.UtcNow)
        {
            state = State.Published;
        }

        evnt.Publication.Reason = reason;
        evnt.Publication.State = state;

        SendEmailStatusChange(evnt, reason);

        return evntRepo.Update(eventId, evnt);
    }

    private void SendEmailStatusChange(Event evnt, string? reason)
    {
        var evntName = evnt.Publication.Title!;
        if (evntName.Length > MAX_TITLE_LENGTH)
            evntName = evntName.Substring(0, MAX_TITLE_LENGTH) + "...";

        string subject;
        string statusStr;
        switch (evnt.Publication.State)
        {
            case State.Approved:
                statusStr = "approuvée";
                break;
            case State.Denied:
                statusStr = "refusée";
                break;
            case State.Published:
                statusStr = "publiée";
                break;
            case State.Deleted:
                statusStr = "supprimée";
                break;
            default:
                statusStr = "changement de status";
                break;
        }
        subject = $"Publication {statusStr} - {evntName}";

        emailService.SendEmailAsync(
            evnt.Publication.Organizer.Email,
            subject,
            new StatusChangeModel
            {
                Title = "Changement de status de votre publication",
                Salutation = $"Bonjour {evnt.Publication.Organizer.Organization},",
                StatusHeaderText = $"La publication « ­{evnt.Publication.Title} » a été placée dans le status ",
                StatusNameText = statusStr,
                StatusRefusalReason = reason,
                StatusRefusalHeader = "Raison du changement:",
                ButtonSeePublicationText = "Voir la publication",
                ButtonLink = new Uri("https://www.google.com/"), // TODO: add link to publication
            },
            EmailsUtils.StatusChangeTemplate);
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

    public bool UpdateEventReportCount(Guid eventId)
    {
        var evnt = evntRepo.Get(eventId);

        if (evnt == null) return false;

        evnt.Publication.ReportCount++;

        return evntRepo.Update(eventId, evnt);
    }

    public bool UpdatePublicationHasBeenReported(Guid eventId)
    {
        var evnt = evntRepo.Get(eventId);

        if (evnt == null) return false;

        evnt.Publication.HasBeenReported = true;

        return evntRepo.Update(eventId, evnt);
    }

}
