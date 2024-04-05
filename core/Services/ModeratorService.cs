using api.core.data.entities;
using api.core.Data.Exceptions;
using api.core.Data.Requests;
using api.core.repositories.abstractions;
using api.core.services.abstractions;

namespace api.core.Services;

public class ModeratorService(IConfiguration configuration, IModeratorRepository moderatorRepository) : IModeratorService
{
    public ModeratorResponseDTO CreateModerator(string apiKey, ModeratorCreateRequestDTO req)
    {
        var adminAccessKey = Environment.GetEnvironmentVariable("ADMIN_ACCESS_API_KEY");
        if (adminAccessKey == null)
            throw new Exception("Admin access key not defined, can't trigger this function");

        if (apiKey != adminAccessKey)
            throw new UnauthorizedException();

        // Create the moderator
        moderatorRepository.Add(new Moderator
        {
            Id = req.Id,
            Email = req.Email,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
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
