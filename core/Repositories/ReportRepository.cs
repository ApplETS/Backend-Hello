using api.core.data;
using api.core.data.entities;
using api.core.repositories.abstractions;

using Microsoft.EntityFrameworkCore;

namespace api.core.repositories;

public class ReportRepository(EventManagementContext context) : IReportRepository
{
    public Report Add(Report entity)
    {
        var inserted = context.Reports.Add(entity);

        if (inserted.Entity != null)
        {
            context.SaveChanges();
            return inserted.Entity;
        }
        throw new Exception($"Unable to create a Report with id: {entity.Id}");
    }

    public bool Delete(Report entity)
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

    public Report? Get(Guid id)
    {
        var report = context.Reports
            .Include(r => r.Publication)
            .FirstOrDefault(x => x.Id == id);

        return report != null ? report : throw new Exception($"Unable to fetch report with id: {id}");
    }

    public IQueryable<Report> GetAll()
    {
        return context.Reports
            .Include(r => r.Publication);
    }

    public IEnumerable<Report> GetRecentReports(int lastSeconds)
    {
        return context.Reports
            .Where(r => r.CreatedAt >= DateTime.UtcNow.AddSeconds(-lastSeconds))
            .Include(r => r.Publication);
    }

    public bool Update(Guid id, Report entity)
    {
        var report = Get(id);

        if (report != null)
        {
            context.Entry(report).CurrentValues.SetValues(entity);
            context.SaveChanges();
            return true;
        }

        return false;
    }
}
