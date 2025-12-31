using System.ComponentModel.DataAnnotations.Schema;

using api.core.Data.Entities;


namespace api.core.data.entities;

[Table("ActivityArea")]
public partial class ActivityArea : BaseEntity
{
    public string NameFr { get; set; } = null!;
    
    public string NameEn { get; set; } = null!;

    /// <summary>
    /// Groups all Users, no matter their role
    /// </summary>
    [InverseProperty(nameof(User.ActivityArea))]
    public virtual ICollection<User> Users { get; set; } = new List<User>();

    [NotMapped]
    public ICollection<User> Organizers => Users.Where(u => u.Role.HasFlag(UserRole.Organizer)).ToList();

    [NotMapped]
    public ICollection<User> Moderators => Users.Where(u => u.Role.HasFlag(UserRole.Moderator)).ToList();
}
