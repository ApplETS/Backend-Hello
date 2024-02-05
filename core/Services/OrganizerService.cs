using api.core.data.entities;
using api.core.Data.Exceptions;
using api.core.Data.requests;
using api.core.Data.Responses;
using api.core.repositories.abstractions;
using api.core.services.abstractions;

namespace api.core.Services;

public class OrganizerService(IOrganizerRepository repository) : IOrganizerService
{
    public OrganizerResponseDTO AddOrganizer(OrganizerRequestDTO organizerDto)
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

    public OrganizerResponseDTO GetOrganizer(Guid id)
    {
        var organizer = repository.Get(id);
        NotFoundException<Organizer>.ThrowIfNull(organizer);

        return OrganizerResponseDTO.Map(organizer!);
    }

    public bool UpdateOrganizer(Guid id, OrganizerRequestDTO dto)
    {
        var originalOrganizer = repository.Get(id);
        NotFoundException<Organizer>.ThrowIfNull(originalOrganizer);

        if (originalOrganizer!.Id != id) throw new UnauthorizedException();

        return repository.Update(id, new Organizer
        {
            Id = dto.Id,
            Name = dto.Name,
            Email = dto.Email,
            Organisation = dto.Organisation,
            ActivityArea = dto.ActivityArea,
            CreatedAt = originalOrganizer.CreatedAt,
            UpdatedAt = DateTime.UtcNow
        });
    }
}
