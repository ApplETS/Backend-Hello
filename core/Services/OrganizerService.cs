using api.core.data.entities;
using api.core.Data.requests;
using api.core.Data.Responses;
using api.core.repositories.abstractions;
using api.core.services.abstractions;

namespace api.core.Services;

public class OrganizerService(IOrganizerRepository repository) : IOrganizerService
{
    public OrganizerResponseDTO AddOrganizer(OrganizerCreationRequestDTO organizerDto)
    {
        var inserted = repository.Add(new Organizer
        {
            Id = organizerDto.Id,
            Name = organizerDto.Name,
            Email = organizerDto.Email,
            Organisation = organizerDto.Organisation,
            ActivityArea = organizerDto.ActivityArea,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        return OrganizerResponseDTO.Map(inserted);
    }
}
