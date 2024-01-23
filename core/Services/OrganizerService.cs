using api.core.Data.requests;
using api.core.Data.Responses;
using api.core.repositories.abstractions;
using api.core.services.abstractions;

namespace api.core.Services;

public class OrganizerService(IOrganizerRepository repository) : IOrganizerService
{
    public OrganizerResponseDTO AddOrganizer(OrganizerCreationRequestDTO organizerDto)
    {
        var inserted = repository.AddOrganizer(organizerDto);
        return OrganizerResponseDTO.Map(inserted);
    }
}
