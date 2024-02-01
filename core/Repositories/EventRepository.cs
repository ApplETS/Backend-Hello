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
        throw new NotImplementedException();
    }

    public bool Delete(Event entity)
    {
        throw new NotImplementedException();
    }

    public Event? Get(Guid id)
    {
        var event1 = context.Events
            .Include(x => x.Publication)
                .ThenInclude(x => x.Organizer)
            .Include(x => x.Publication)
                .ThenInclude(x => x.Moderator)
            .FirstOrDefault(x => x.Id == id);

        return event1 != null ? event1 : throw new Exception($"Unable to fetch an event {id}");
    }

    public IEnumerable<Event> GetAll()
    {
        throw new NotImplementedException();
    }

    public bool Update(Guid id, Event entity)
    {
        throw new NotImplementedException();
    }
}
