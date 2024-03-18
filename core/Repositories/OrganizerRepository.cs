using api.core.data;
using api.core.data.entities;
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
        var entity = context.Organizers.Find(id);
        if (entity != null && entity.DeletedAt == null)
        {
            return entity;
        }
        return null;
    }

    public IEnumerable<Organizer> GetAll()
    {
        return context.Organizers.ToList();
    }

    public bool Update(Guid id, Organizer entity)
    {
        var existingEntity = Get(id);

        if (existingEntity != null)
        {
            context.Entry(existingEntity).CurrentValues.SetValues(entity);
            context.SaveChanges();
            return true;
        }

        return false;
    }
}
