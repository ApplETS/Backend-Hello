using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using api.core.Data.Enums;

using Microsoft.EntityFrameworkCore;

namespace api.core.data.entities;

[Table("Report")]
[Index("PublicationId", Name = "IX_Report_PublicationId")]
public partial class Report
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }

    public string Reason { get; set; } = null!;

    public ReportCategory Category { get; set; }

    public Guid PublicationId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    [ForeignKey("PublicationId")]
    [InverseProperty("Reports")]
    public virtual Publication Publication { get; set; } = null!;
}
