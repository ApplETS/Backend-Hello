using System.ComponentModel.DataAnnotations.Schema;

using api.core.Data.Entities;


namespace api.core.data.entities;

[Table("Tag")]
public partial class Tag : BaseEntity
{
    public string Name { get; set; } = null!;

    public int PriorityValue { get; set; }

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
