using api.core.data;
using api.core.data.entities;
using api.core.Data.requests;
using api.core.repositories.abstractions;

namespace api.core.repositories;

public class OrganizerRepository(EventManagementContext context) : IOrganizerRepository
{
    public Organizer AddOrganizer(OrganizerCreationRequestDTO organizer)
    {
        var inserted = context.Organizers.Add(new Organizer
        {
            Id = organizer.Id,
            Name = organizer.Name,
            Email = organizer.Email,
            Organisation = organizer.Organisation,
            ActivityArea = organizer.ActivityArea,
        });
        if(inserted.Entity != null)
        {
            context.SaveChanges();
            return inserted.Entity;
        }
        throw new Exception($"Unable to create an organizer {organizer.Id}");
    }
}
