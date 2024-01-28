using api.core.data.entities;
using api.core.Data.requests;

namespace api.core.repositories.abstractions;

public interface IOrganizerRepository
{
    public Organizer AddOrganizer(OrganizerCreationRequestDTO organizer);
}
