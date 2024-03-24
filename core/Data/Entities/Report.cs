using System.ComponentModel.DataAnnotations.Schema;

using api.core.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace api.core.data.entities;

[Table("Report")]
[Index("PublicationId", Name = "IX_Report_PublicationId")]
public partial class Report : BaseEntity
{
    public string Reason { get; set; } = null!;

    public DateTime Date { get; set; }

    public Guid PublicationId { get; set; }

    [ForeignKey("PublicationId")]
    [InverseProperty("Reports")]
    public virtual Publication Publication { get; set; } = null!;
}
