using api.core.data.entities;

namespace api.core.Data.Responses;

public class ActivityAreaResponseDTO
{
    public Guid Id { get; set; }

    public string? NameFr { get; set; } = null!;

    public string? NameEn { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public static ActivityAreaResponseDTO Map(ActivityArea aArea)
    {
        return new ActivityAreaResponseDTO
        {
            Id = aArea.Id,
            NameFr = aArea.NameFr,
            NameEn = aArea.NameEn,
            CreatedAt = aArea.CreatedAt,
            UpdatedAt = aArea.UpdatedAt,
        };
    }
}
