using System.ComponentModel.DataAnnotations.Schema;

using api.core.Data.Entities;


namespace api.core.data.entities;

[Table(nameof(Notification))]
public partial class Notification : BaseEntity
{
    public Guid PublicationId { get; set; }
    
    public Guid SubscriptionId { get; set; }

    public bool IsSent { get; set; } = false;

    [ForeignKey(nameof(PublicationId))]
    [InverseProperty(nameof(Publication.Notifications))]
    public virtual Publication Publication { get; set; } = null!;

    [ForeignKey(nameof(SubscriptionId))]
    [InverseProperty(nameof(Subscription.Notifications))]
    public virtual Subscription Subscription { get; set; } = null!;
}
