namespace api.core.Data.requests;

public class OrganizerCreationRequestDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Organisation { get; set; } = null!;

    public string ActivityArea { get; set; } = null!;
}
