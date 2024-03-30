using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.core.data.entities;

[Table("Event")]
public partial class Event
{
    [Key]
    public Guid Id { get; set; }

    public DateTime? EventStartDate { get; set; }
    public DateTime? EventEndDate { get; set; }

    public int ReportCount { get; set; } = 0;

    [ForeignKey("Id")]
    [InverseProperty("Event")]
    public virtual Publication Publication { get; set; } = null!;
}
