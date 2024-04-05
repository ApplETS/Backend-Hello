using api.core.data;
using api.core.data.entities;
using api.core.Data.Responses;
using api.core.repositories.abstractions;

using Microsoft.EntityFrameworkCore;

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

    public IEnumerable<FieldOfInterestTagResponseDTO> GetInterestFieldsForOrganizer(Guid organizerId, int take = 3)
    {
        return context.Tags
            .Include(x => x.Publications)
            .Where(x => x.Publications.Any(p => p.OrganizerId == organizerId))
            .GroupBy(x => x.Id)
            .Select(grp => new FieldOfInterestTagResponseDTO
            {
                Id = grp.Key,
                Name = grp.First().Name,
                CreatedAt = grp.First().CreatedAt,
                UpdatedAt = grp.First().UpdatedAt,
                Count = grp.Count(),
            }).OrderByDescending(x => x.Count)
            .Take(take);
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
