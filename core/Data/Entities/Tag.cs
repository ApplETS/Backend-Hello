using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace api.core.Data.Entities;

[Table("Tag")]
public partial class Tag
{
    [Key]
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public int PriorityValue { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    [ForeignKey("ParentTagsId")]
    [InverseProperty("ParentTags")]
    public virtual ICollection<Tag> ChildrenTags { get; set; } = new List<Tag>();

    [ForeignKey("ChildrenTagsId")]
    [InverseProperty("ChildrenTags")]
    public virtual ICollection<Tag> ParentTags { get; set; } = new List<Tag>();

    [ForeignKey("TagsId")]
    [InverseProperty("Tags")]
    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
}
