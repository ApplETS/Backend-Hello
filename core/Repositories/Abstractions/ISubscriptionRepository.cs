using api.core.data.entities;

namespace api.core.repositories.abstractions;

public interface ISubscriptionRepository : IRepository<Subscription>
{
    public bool IsEntryExists(string organizerId, string email);
}
