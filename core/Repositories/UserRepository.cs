using api.core.data;
using api.core.data.entities;
using api.core.Data.Entities;
using api.core.repositories.abstractions;
using api.core.Repositories.Abstractions;

namespace api.core.repositories;

public class UserRepository(EventManagementContext context) : IUserRepository
{
    public User Add(User entity)
    {
        var inserted = context.Users.Add(entity);

        if (inserted.Entity != null)
        {
            context.SaveChanges();
            return inserted.Entity;
        }
        throw new Exception($"Unable to create a Moderator {entity.Id}");
    }

    public bool Delete(User entity)
    {
        throw new NotImplementedException();
    }

    public User? Get(string id)
    {
        var entity = context.Users.Find(id);
        if (entity != null && entity.DeletedAt == null)
        {
            return entity;
        }
        return null;
    }

    public IQueryable<User> GetAll()
    {
        throw new NotImplementedException();
    }

    public bool Update(string id, User entity)
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

    public User? GetOrganizer(string id)
    {
        var user = Get(id);
        if (user == null || !user.Role.HasFlag(UserRole.Organizer))
        {
            return null;
        }

        return user;
    }

    public User? GetModerator(string id)
    {
        var user = Get(id);
        if (user == null || !user.Role.HasFlag(UserRole.Moderator))
        {
            return null;
        }

        return user;
    }
}
