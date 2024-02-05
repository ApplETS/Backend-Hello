namespace api.core.Data.requests;

public class OrganizerRequestDTO
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }

    public string Organisation { get; set; } = null!;

    public string ActivityArea { get; set; } = null!;
}
