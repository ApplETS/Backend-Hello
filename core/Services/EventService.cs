using api.core.data.entities;
using api.core.Data.Exceptions;
using api.core.Data.requests;
using api.core.Data.Responses;
using api.core.repositories.abstractions;
using api.core.Repositories.Abstractions;
using api.core.services.abstractions;

using Microsoft.IdentityModel.Tokens;

namespace api.core.Services;

public class EventService(IEventRepository evntRepo, ITagRepository tagRepo, IOrganizerRepository orgRepo) : IEventService
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
                Tags = (ICollection<Tag>) tags,
                Organizer = organizer,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
        });
        
        return EventResponseDTO.Map(inserted);
    }
}
