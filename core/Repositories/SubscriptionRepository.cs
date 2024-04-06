using api.core.data;
using api.core.data.entities;
using api.core.repositories.abstractions;

namespace api.core.repositories;

public class SubscriptionRepository(EventManagementContext context) : ISubscriptionRepository
{
    public Subscription Add(Subscription entity)
    {
        var inserted = context.Subscriptions.Add(entity);

        if (inserted.Entity != null)
        {
            context.SaveChanges();
            return inserted.Entity;
        }
        throw new Exception($"Unable to create a subscriptions {entity.Id}");
    }

    public bool Delete(Subscription entity)
    {
        try
        {
            context.Remove(entity);
            context.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Subscription? Get(Guid id)
    {
        var act = context.Subscriptions
            .FirstOrDefault(x => x.Id == id);

        return act ?? throw new Exception($"Unable to fetch a Subscription {id}");
    }

    public IQueryable<Subscription> GetAll()
    {
        return context.Subscriptions;
    }

    public bool Update(Guid id, Subscription entity)
    {
        var sub = Get(id);

        if (sub != null)
        {
            context.Entry(sub).CurrentValues.SetValues(entity);
            context.SaveChanges();
            return true;
        }

        return false;
    }

    public bool IsEntryExists(Guid organizerId, string email)
    {
        return context.Subscriptions
            .Any(x => x.OrganizerId == organizerId && x.Email == email);
    }
}
