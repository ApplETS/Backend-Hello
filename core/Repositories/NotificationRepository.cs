using api.core.data;
using api.core.data.entities;
using api.core.repositories.abstractions;

namespace api.core.repositories;

public class NotificationRepository(EventManagementContext context) : INotificationRepository
{
    public Notification Add(Notification entity)
    {
        var inserted = context.Notifications.Add(entity);

        if (inserted.Entity != null)
        {
            context.SaveChanges();
            return inserted.Entity;
        }
        throw new Exception($"Unable to create a Notification {entity.Id}");
    }
    public bool SoftDelete(Notification entity)
    {
        try
        {
            entity.DeletedAt = DateTime.UtcNow;
            context.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool Delete(Notification entity)
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

    public Notification? Get(Guid id)
    {
        var act = context.Notifications
            .FirstOrDefault(x => x.Id == id);

        return act ?? throw new Exception($"Unable to fetch a Notification {id}");
    }

    public IQueryable<Notification> GetAll()
    {
        return context.Notifications;
    }

    public bool Update(Guid id, Notification entity)
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
}
