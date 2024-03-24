using api.core.data;
using api.core.data.entities;
using api.core.Data.requests;
using api.core.repositories.abstractions;

using Microsoft.EntityFrameworkCore;

namespace api.core.repositories;

public class EventRepository(EventManagementContext context) : IEventRepository
{
    public Event Add(Event entity)
    {
        var inserted = context.Events.Add(entity);

        if (inserted.Entity != null)
        {
            context.SaveChanges();
            return inserted.Entity;
        }
        throw new Exception($"Unable to create an event {entity.Id}");
    }

    public bool Delete(Event entity)
    {
        try
        {
            entity.Publication.DeletedAt = DateTime.UtcNow;
            context.Update(entity.Publication);
            context.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Event? Get(Guid id)
    {
        var evnt = context.Events
            .Include(x => x.Publication)
                .ThenInclude(x => x.Organizer)
            .Include(x => x.Publication)
                .ThenInclude(x => x.Moderator)
             .Include(x => x.Publication)
                .ThenInclude(x => x.Tags)
            .FirstOrDefault(x => x.Id == id);

        return evnt != null ? evnt : throw new Exception($"Unable to fetch an event {id}");
    }

    public IEnumerable<Event> GetAll()
    {
        return context.Events
            .Include(x => x.Publication)
                .ThenInclude(x => x.Tags)
            .Include(x => x.Publication)
                .ThenInclude(x => x.Organizer);
    }

    public void ResetTags(Guid eventId)
    {
        var evnt = Get(eventId);

        if (evnt!.Publication != null)
        {
            evnt!.Publication!.Tags.Clear();
            context.SaveChanges();
        }
    }

    public bool Update(Guid id, Event entity)
    {
        var evnt = Get(id);

        if (evnt != null)
        {
            context.Entry(evnt).CurrentValues.SetValues(entity);
            context.Entry(evnt.Publication).CurrentValues.SetValues(entity.Publication);
            context.SaveChanges();
            return true;
        }

        return false;
    }
}
