using api.core.data;
using api.core.data.entities;
using api.core.repositories.abstractions;

namespace api.core.repositories;

public class ModeratorRepository(EventManagementContext context) : IModeratorRepository
{
    public Moderator Add(Moderator entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(Moderator entity)
    {
        throw new NotImplementedException();
    }

    public Moderator? Get(Guid id)
    {
        var entity = context.Moderators.Find(id);
        if (entity != null && entity.DeletedAt == null)
        {
            return entity;
        }
        return null;
    }

    public IEnumerable<Moderator> GetAll()
    {
        throw new NotImplementedException();
    }

    public bool Update(Guid id, Moderator entity)
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
