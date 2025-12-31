using System.ComponentModel.DataAnnotations.Schema;

using api.core.Data.Entities;


namespace api.core.data.entities;

[Table(nameof(Subscription))]
public partial class Subscription : BaseEntity
{
    public string Email { get; set; } = null!;

    public string OrganizerId { get; set; } = null!;

    public string SubscriptionToken { get; set; } = null!;

    [ForeignKey(nameof(OrganizerId))]
    [InverseProperty(nameof(User.Subscriptions))]
    public virtual User Organizer { get; set; } = null!;

    [InverseProperty(nameof(Notification.Subscription))]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
