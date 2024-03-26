using System.ComponentModel.DataAnnotations.Schema;

using api.core.Data.Entities;

namespace api.core.data.entities;

[Table("Moderator")]
public partial class Moderator : User
{
    [InverseProperty("Moderator")]
    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
}
