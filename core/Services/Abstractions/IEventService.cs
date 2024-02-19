using api.core.Data.Entities;
using api.core.Data.requests;
using api.core.Data.Responses;

namespace api.core.services.abstractions;

public interface IEventService
{
    public IEnumerable<EventResponseDTO> GetEvents(DateTime? startDate, DateTime? endDate, IEnumerable<string>? activityAreas, IEnumerable<Guid>? tags, Guid? userId, State state, bool ignorePublicationDate = false);

    public EventResponseDTO GetEvent(Guid id);

    public EventResponseDTO AddEvent(Guid userId, EventRequestDTO request);

    public bool DeleteEvent(Guid userId, Guid eventId);

    public bool UpdateEvent(Guid userId, Guid eventId, EventRequestDTO request);

    public bool UpdateEventState(Guid userId, Guid eventId, State state);
}
