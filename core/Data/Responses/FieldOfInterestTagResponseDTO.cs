namespace api.core.Data.Responses;

public class FieldOfInterestTagResponseDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int Count { get; set; }
}
