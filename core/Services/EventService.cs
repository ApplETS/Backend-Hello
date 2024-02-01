using api.core.Data.requests;
using api.core.Data.Responses;
using api.core.repositories.abstractions;
using api.core.services.abstractions;

namespace api.core.Services;

public class EventService(IEventRepository repository) : IEventService
{
    public EventResponseDTO GetEvents(DateTime? startDate, DateTime? endDate, IEnumerable<string>? tags)
    {
        return EventResponseDTO.Map(repository.GetOne(Guid.NewGuid()));
    }
}
