using api.core.data.entities;
using api.core.Data.Entities;
using api.core.Data.Exceptions;
using api.core.Data.Requests;
using api.core.repositories.abstractions;
using api.core.Repositories.Abstractions;
using api.core.services.abstractions;

namespace api.core.Services;

public class ModeratorService(IUserRepository userRepository, IConfiguration configuration) : IModeratorService
{
    public ModeratorResponseDTO CreateModerator(string apiKey, ModeratorCreateRequestDTO req)
    {
        var adminAccessKey = configuration.GetValue<string>("ADMIN_ACCESS_API_KEY");
        if (adminAccessKey == null)
            throw new Exception("Admin access key not defined, can't trigger this function");

        if (apiKey != adminAccessKey)
            throw new UnauthorizedException();

        // Create the moderator
        userRepository.Add(new User
        {
            Id = req.Id,
            Email = req.Email,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Role = UserRole.Moderator
        });

        return new ModeratorResponseDTO
        {
            Id = req.Id,
            Email = req.Email,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
