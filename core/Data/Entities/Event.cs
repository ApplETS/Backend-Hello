using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.core.data.entities;

[Table("Event")]
public partial class Event
{
    [Key]
    public Guid Id { get; set; }

    public DateTime EventDate { get; set; }

    [ForeignKey("Id")]
    [InverseProperty("Event")]
    public virtual Publication Publication { get; set; } = null!;
}
