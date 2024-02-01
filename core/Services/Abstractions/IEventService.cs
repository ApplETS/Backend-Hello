using api.core.Data.requests;
using api.core.Data.Responses;

namespace api.core.services.abstractions;

public interface IEventService
{
    public EventResponseDTO GetEvents(DateTime? startDate, DateTime? endDate, IEnumerable<string>? tags);
}
