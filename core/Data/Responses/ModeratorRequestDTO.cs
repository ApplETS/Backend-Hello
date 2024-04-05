namespace api.core.Data.Requests;

public class ModeratorResponseDTO
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
