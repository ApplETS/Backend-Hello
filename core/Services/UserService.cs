using api.core.data.entities;
using api.core.Data.Exceptions;
using api.core.Data.requests;
using api.core.Data.Responses;
using api.core.repositories.abstractions;
using api.core.services.abstractions;

namespace api.core.Services;

public class UserService(IOrganizerRepository organizerRepository, IModeratorRepository moderatorRepository) : IUserService
{
    public UserResponseDTO AddOrganizer(UserRequestDTO organizerDto)
    {
        var inserted = organizerRepository.Add(new Organizer
        {
            Name = organizerDto.Name,
            Email = organizerDto.Email,
            Organisation = organizerDto.Organisation ?? "",
            ActivityArea = organizerDto.ActivityArea ?? "",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        return UserResponseDTO.Map(inserted);
    }

    public UserResponseDTO GetUser(Guid id)
    {
        var organizer = organizerRepository.Get(id);
        if (organizer != null)
            return UserResponseDTO.Map(organizer!);

        var moderator = moderatorRepository.Get(id);
        if (moderator != null)
            return UserResponseDTO.Map(moderator!);

        throw new Exception("No users associated with this ID");
    }

    public bool UpdateUser(Guid id, UserRequestDTO dto)
    {
        var user = GetUser(id);

        switch (user.Type)
        {
            case "Moderator":
                return moderatorRepository.Update(id, new Moderator
                {
                    Id = id,
                    Name = dto.Name,
                    Email = dto.Email,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = DateTime.UtcNow
                });
            case "Organizer":
                return organizerRepository.Update(id, new Organizer
                {
                    Id = id,
                    Name = dto.Name,
                    Email = dto.Email,
                    Organisation = dto.Organisation,
                    ActivityArea = dto.ActivityArea,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = DateTime.UtcNow
                });
            default:
                throw new Exception("No users associated witht thid ID can be updated");
        }

        
    }
}
