using api.core.Data.requests;
using api.core.Data.Responses;

namespace api.core.services.abstractions;

public interface IOrganizerService
{
    public OrganizerResponseDTO AddOrganizer(OrganizerRequestDTO organizerDto);

    public OrganizerResponseDTO GetOrganizer(Guid id);

    public bool UpdateOrganizer(Guid id, OrganizerRequestDTO dto);
}
