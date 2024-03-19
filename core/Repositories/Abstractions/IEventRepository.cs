using api.core.data.entities;

namespace api.core.repositories.abstractions;

public interface IEventRepository : IRepository<Event>
{
    public void ResetTags(Guid eventId);
}
