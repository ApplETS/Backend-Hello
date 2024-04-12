namespace api.core.Data.requests;

public class SubscribeRequestDTO
{
    public required string Email { get; set; }

    public required Guid OrganizerId { get; set; }
}


public class UnsubscribeRequestDTO
{
    public required string SubscriptionToken { get; set; }

}