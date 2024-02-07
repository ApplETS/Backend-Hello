using api.core.data.entities;
using api.core.Data.Exceptions;
using api.core.Data.requests;
using api.core.Data.Responses;
using api.core.repositories.abstractions;
using api.core.services.abstractions;

using Microsoft.IdentityModel.Tokens;

namespace api.core.Services;

public class EventService(IEventRepository evntRepo, ITagRepository tagRepo, IOrganizerRepository orgRepo, IModeratorRepository moderatorRepo) : IEventService
{
    public IEnumerable<EventResponseDTO> GetEvents(
        DateTime? startDate,
        DateTime? endDate,
        IEnumerable<string>? activityAreas,
        IEnumerable<Guid>? tags)
    {
        var events = evntRepo.GetAll();

        return events.Where(e =>
         e.Publication.DeletedAt == null &&
         (startDate == null || e.EventDate >= startDate) &&
         (endDate == null || e.EventDate <= endDate) &&  
         (tags.IsNullOrEmpty() || e.Publication.Tags.Any(t => tags.Any(tt => t.Id == tt))) &&
         (activityAreas.IsNullOrEmpty() || activityAreas.Any(aa => aa == e.Publication.Organizer.ActivityArea)))
            .OrderBy(e => e.EventDate)
            .Select(EventResponseDTO.Map);
    }

    public EventResponseDTO GetEvent(Guid id)
    {
        var evnt = evntRepo.Get(id);
        NotFoundException<Event>.ThrowIfNull(evnt);

        return EventResponseDTO.Map(evnt);
    }

    public EventResponseDTO AddEvent(Guid userId, EventRequestDTO request)
    {
        var organizer = orgRepo.Get(userId) ?? throw new UnauthorizedException();

        var tags = tagRepo.GetAll()
            .Where(t => request.Tags.Contains(t.Id))
            ?? Enumerable.Empty<Tag>();

        var inserted = evntRepo.Add(new Event
        {
            EventDate = request.EventDate,
            Publication = new Publication
            {
                Title = request.Title,
                Content = request.Content,
                ImageUrl = request.ImageUrl,
                State = request.State,
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


    public bool UpdateEvent(Guid userId, Guid eventId, EventRequestDTO request)
    {
        var organizer = orgRepo.Get(userId) ?? throw new UnauthorizedException();
        var evnt = evntRepo.Get(eventId);

        if (evnt!.Publication.Organizer != null && evnt.Publication.Organizer.Id != userId)
            throw new UnauthorizedException();

        var tags = tagRepo.GetAll()
            .Where(t => request.Tags.Contains(t.Id))
            ?? Enumerable.Empty<Tag>();

        return evntRepo.Update(eventId, new Event
        {
            Id = eventId,
            EventDate = request.EventDate,
            Publication = new Publication
            {
                Id = eventId,
                Title = request.Title,
                Content = request.Content,
                ImageUrl = request.ImageUrl,
                State = request.State,
                PublicationDate = request.PublicationDate,
                Tags = tags.ToList(),
                Organizer = organizer,
                OrganizerId = organizer.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
        });
    }

    public bool UpdateEventState(Guid userId, Guid eventId, string state)
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
}
