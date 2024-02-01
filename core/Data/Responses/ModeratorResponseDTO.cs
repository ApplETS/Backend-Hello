using api.core.data.entities;

namespace api.core.Data.Responses;

public class ModeratorResponseDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public static ModeratorResponseDTO Map(Moderator moderator)
    {
        return new ModeratorResponseDTO
        {
            Id = moderator.Id,
            Name = moderator.Name,
            Email = moderator.Email,
        };
    }
}
