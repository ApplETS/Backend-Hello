using api.core.Data.requests;
using api.core.data.entities;
using api.core.repositories.abstractions;
using api.core.Services.Abstractions;

using api.core.Data.Exceptions;

namespace api.core.Services;

public class SubscriptionService(ISubscriptionRepository subscriptionRepository) : ISubscriptionService
{
    public void Subscribe(SubscribeRequestDTO subscribeRequest)
    {
        var token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString();
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(token);
        var encodedToken = Convert.ToBase64String(plainTextBytes);

        if (subscriptionRepository.IsEntryExists(subscribeRequest.OrganizerId, subscribeRequest.Email))
        {
            // early return if entry already exists, we don't want to throw an exception here
            // Since an attacker could use this to check if an email is subscribed or not
            return;
        }

        subscriptionRepository.Add(new Subscription
        {
            Email = subscribeRequest.Email,
            OrganizerId = subscribeRequest.OrganizerId,
            SubscriptionToken = encodedToken,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });
    }

    public void Unsubscribe(UnsubscribeRequestDTO unsubscribeRequest)
    {
        var subscription = subscriptionRepository.GetAll()
                                .FirstOrDefault(x => x.SubscriptionToken == unsubscribeRequest.SubscriptionToken);
        NotFoundException<Subscription>.ThrowIfNull(subscription);

        subscriptionRepository.Delete(subscription!);
    }

    public void SendNewsForOrganizerSubscribers(Guid organizerId, string subject, string content)
    {
        var subscriptions = subscriptionRepository.GetAll()
                                .Where(x => x.OrganizerId == organizerId)
                                .ToList();

        foreach (var subscription in subscriptions)
        {
            // send email to subscription.Email with subject and content
        }
    }
}
