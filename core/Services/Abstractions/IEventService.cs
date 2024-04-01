using api.core.Data.Enums;
using api.core.Data.requests;
using api.core.Data.Responses;

namespace api.core.services.abstractions;

public interface IEventService
{
    public IEnumerable<EventResponseDTO> GetEvents(DateTime? startDate, DateTime? endDate, IEnumerable<string>? activityAreas, IEnumerable<Guid>? tags, Guid? organizerId, State state, bool ignorePublicationDate = false);

    public EventResponseDTO GetEvent(Guid id);

    public EventResponseDTO AddEvent(Guid userId, EventCreationRequestDTO request);

    public EventResponseDTO AddDraftEvent(Guid userId, DraftEventCreationRequestDTO dto);

    public bool DeleteEvent(Guid userId, Guid eventId);

    public bool UpdateEvent(Guid userId, Guid eventId, EventUpdateRequestDTO request);

    public bool UpdateEventState(Guid userId, Guid eventId, State state, string? reason);

    public bool UpdateEventReportCount(Guid eventId);

    public bool UpdateEventHasBeenReported(Guid eventId);

    public int PublishedIfApprovedPassedDue();
}
