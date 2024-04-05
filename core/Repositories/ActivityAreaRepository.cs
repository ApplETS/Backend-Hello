using api.core.data;
using api.core.data.entities;
using api.core.repositories.abstractions;

namespace api.core.repositories;

public class ActivityAreaRepository(EventManagementContext context) : IActivityAreaRepository
{
    public ActivityArea Add(ActivityArea entity)
    {
        var inserted = context.ActivityAreas.Add(entity);

        if (inserted.Entity != null)
        {
            context.SaveChanges();
            return inserted.Entity;
        }
        throw new Exception($"Unable to create an activityAreas {entity.Id}");
    }

    public bool Delete(ActivityArea entity)
    {
        try
        {
            entity.DeletedAt = DateTime.Now;
            context.Update(entity);
            context.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public ActivityArea? Get(Guid id)
    {
        var act = context.ActivityAreas
            .FirstOrDefault(x => x.Id == id);

        return act ?? throw new Exception($"Unable to fetch an ActivityArea {id}");
    }

    public IEnumerable<ActivityArea> GetAll()
    {
        return context.ActivityAreas;
    }

    public bool Update(Guid id, ActivityArea entity)
    {
        var act = Get(id);

        if (act != null)
        {
            context.Entry(act).CurrentValues.SetValues(entity);
            context.SaveChanges();
            return true;
        }

        return false;
    }
}
