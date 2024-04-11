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
    IImageService imageService) : IDraftEventService
{
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
            imageService.EnsureImageSizeAndStore(id.ToString(), request.Image, ImageType.Publication, null);
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
            imageUrl = fileShareService.FileGetDownloadUri($"{evnt.Id}/{request.Image!.FileName}").ToString();
            imageService.EnsureImageSizeAndStore(evnt.Id.ToString(), request.Image!, ImageType.Publication, null);
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
}
