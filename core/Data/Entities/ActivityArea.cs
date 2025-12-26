using System.ComponentModel.DataAnnotations.Schema;

using api.core.Data.Entities;


namespace api.core.data.entities;

[Table("ActivityArea")]
public partial class ActivityArea : BaseEntity
{
    public string NameFr { get; set; } = null!;
    
    public string NameEn { get; set; } = null!;

    //[InverseProperty("ActivityArea")]
    public virtual ICollection<User> Organizers { get; set; } = new List<User>();

    //[InverseProperty("ActivityArea")]
    public virtual ICollection<User> Moderators { get; set; } = new List<User>();
}
