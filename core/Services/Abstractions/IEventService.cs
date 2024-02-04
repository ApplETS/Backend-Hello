using api.core.Data.requests;
using api.core.Data.Responses;

namespace api.core.services.abstractions;

public interface IEventService
{
    public IEnumerable<EventResponseDTO> GetEvents(DateTime? startDate, DateTime? endDate, IEnumerable<string>? activityAreas, IEnumerable<Guid>? tags);
}
