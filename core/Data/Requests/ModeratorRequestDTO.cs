namespace api.core.Data.Requests;

public class ModeratorCreateRequestDTO
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
}
