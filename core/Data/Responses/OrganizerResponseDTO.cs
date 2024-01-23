using api.core.data.entities;

namespace api.core.Data.Responses;

public class OrganizerResponseDTO
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Organisation { get; set; } = null!;

    public string ActivityArea { get; set; } = null!;

    public static OrganizerResponseDTO Map(Organizer organizer)
    {
        return new OrganizerResponseDTO
        {
            Id = organizer.Id,
            Name = organizer.Name,
            Email = organizer.Email,
            Organisation = organizer.Organisation,
            ActivityArea = organizer.ActivityArea,
        };
    }
}
