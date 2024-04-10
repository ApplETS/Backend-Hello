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
using api.core.Services.Abstractions;

namespace api.core.Services;

public class DraftEventService(
    IEventRepository evntRepo,
    ITagService tagService,
    IOrganizerRepository orgRepo,
    IFileShareService fileShareService,
    IEmailService emailService) : IDraftEventService
{
    private const double IMAGE_RATIO_SIZE_ACCEPTANCE = 2.0; // width/height ratio
    private const double TOLERANCE_ACCEPTABILITY = 0.01;
    private const int MAX_TITLE_LENGTH = 15;

    public EventResponseDTO AddDraftEvent(Guid userId, DraftEventRequestDTO request)
    {
        var organizer = orgRepo.Get(userId) ?? throw new UnauthorizedException();

        if (request.Tags.Count > 5)
            throw new BadParameterException<Event>(nameof(request.Tags), "Too many tags");

        var id = Guid.NewGuid();
        Uri? uri = null;

        if (request.Image != null)
        {
            uri = fileShareService.FileGetDownloadUri($"{id}/{request.Image.FileName}");
            HandleImageSaving(id, request.Image);
        }

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

    public bool UpdateDraftEvent(Guid userId, Guid eventId, DraftEventRequestDTO request)
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
            HandleImageSaving(eventId, request.Image!);
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

    private static bool CanPerformAction(Guid userId, Event evnt)
    {
        return (evnt!.Publication.Moderator != null && evnt.Publication.Moderator.Id == userId) ||
            (evnt!.Publication.Organizer != null && evnt.Publication.Organizer.Id == userId);
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
                throw new BadParameterException<Event>(nameof(image), $"Invalid image aspect ratio {width}/{height}");
            
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
