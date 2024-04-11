using api.core.Data.Enums;
using api.core.Data.requests;
using api.core.Data.Responses;
using System.Collections.Generic;
using System;

namespace api.core.services.abstractions;

public interface IEventService
{
    public IEnumerable<EventResponseDTO> GetEvents(DateTime? startDate, DateTime? endDate, IEnumerable<Guid>? activityAreas, IEnumerable<Guid>? tags, Guid? organizerId, string? title, State state, string orderBy = "EventStartDate", bool desc = false, bool ignorePublicationDate = false);

    public EventResponseDTO GetEvent(Guid id);

    public EventResponseDTO AddEvent(Guid userId, EventCreationRequestDTO request);

    public bool DeleteEvent(Guid userId, Guid eventId);

    public bool UpdateEvent(Guid userId, Guid eventId, EventUpdateRequestDTO request);

    public bool UpdateEventState(Guid userId, Guid eventId, State state, string? reason);

    public bool UpdateEventReportCount(Guid eventId);

    public bool UpdatePublicationHasBeenReported(Guid eventId);

    public int PublishedIfApprovedPassedDue();
}
