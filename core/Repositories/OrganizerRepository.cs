using api.core.data;
using api.core.data.entities;
using api.core.Data.requests;
using api.core.repositories.abstractions;

namespace api.core.repositories;

public class OrganizerRepository(EventManagementContext context) : IOrganizerRepository
{
    public Organizer Add(Organizer entity)
    {
        var inserted = context.Organizers.Add(entity);

        if (inserted.Entity != null)
        {
            context.SaveChanges();
            return inserted.Entity;
        }
        throw new Exception($"Unable to create an organizer {entity.Id}");
    }

    public bool Delete(Organizer entity)
    {
        throw new NotImplementedException();
    }

    public Organizer? Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Organizer> GetAll()
    {
        throw new NotImplementedException();
    }

    public bool Update(Guid id, Organizer entity)
    {
        throw new NotImplementedException();
    }
}
