using System.ComponentModel.DataAnnotations.Schema;

using api.core.Data.Entities;

using api.core.Data.Enums;

using Microsoft.EntityFrameworkCore;

namespace api.core.data.entities;

[Table("Report")]
[Index("PublicationId", Name = "IX_Report_PublicationId")]
public partial class Report : BaseEntity
{
    public string Reason { get; set; } = null!;

    public ReportCategory Category { get; set; }

    public Guid PublicationId { get; set; }

    [ForeignKey("PublicationId")]
    [InverseProperty("Reports")]
    public virtual Publication Publication { get; set; } = null!;
}
