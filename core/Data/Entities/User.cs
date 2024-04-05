namespace api.core.Data.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = null!;

    public Guid? ActivityAreaId { get; set; }
}
