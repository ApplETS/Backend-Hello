using api.core.Data.requests;
using api.core.Data.Responses;

namespace api.core.services.abstractions;

public interface IOrganizerService
{
    public OrganizerResponseDTO AddOrganizer(OrganizerCreationRequestDTO organizerDto);
}
