namespace api.core.Data.requests;

public class UserRequestDTO
{
    public Guid Id { get; set; }

    public required string Email { get; set; }

    public string? Organisation { get; set; } = null!;

    public string? ActivityArea { get; set; } = null!;
}
