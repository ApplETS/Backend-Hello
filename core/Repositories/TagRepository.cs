using api.core.data;
using api.core.data.entities;
using api.core.repositories.abstractions;

namespace api.core.repositories;

public class TagRepository(EventManagementContext context) : ITagRepository
{
    public Tag Add(Tag entity)
    {
        var inserted = context.Tags.Add(entity);

        if (inserted.Entity != null)
        {
            context.SaveChanges();
            return inserted.Entity;
        }
        throw new Exception($"Unable to create an Tag {entity.Id}");
    }

    public bool Delete(Tag entity)
    {
        try
        {
            entity.DeletedAt = DateTime.UtcNow;
            context.Update(entity);
            context.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Tag? Get(Guid id)
    {
        var tag = context.Tags
            .FirstOrDefault(x => x.Id == id);

        return tag != null ? tag : throw new Exception($"Unable to fetch a tag {id}");
    }

    public IQueryable<Tag> GetAll()
    {
        return context.Tags;
    }

    public bool Update(Guid id, Tag entity)
    {
        var tag = Get(id);

        if (tag != null)
        {
            context.Entry(tag).CurrentValues.SetValues(entity);
            context.SaveChanges();
            return true;
        }

        return false;
    }
}
