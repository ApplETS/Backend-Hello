using System.ComponentModel.DataAnnotations.Schema;

using api.core.Data.Entities;


namespace api.core.data.entities;

[Table("ActivityArea")]
public partial class ActivityArea : BaseEntity
{
    public string NameFr { get; set; } = null!;
    
    public string NameEn { get; set; } = null!;

    [InverseProperty("ActivityArea")]
    public virtual ICollection<Organizer> Organizers { get; set; } = new List<Organizer>();

    [InverseProperty("ActivityArea")]
    public virtual ICollection<Moderator> Moderators { get; set; } = new List<Moderator>();
}
