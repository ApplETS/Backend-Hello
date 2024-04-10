using api.core.Data.requests;
using api.core.data.entities;
using api.core.repositories.abstractions;
using api.core.Services.Abstractions;

using api.core.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using api.emails.Services.Abstractions;
using api.core.repositories;
using api.emails;
using api.emails.Models;

namespace api.core.Services;

public class NotificationService(
    ILogger<NotificationService> logger,
    IEventRepository eventRepository,
    ISubscriptionRepository subscriptionRepository,
    INotificationRepository notificationRepository,
    IEmailService emailService) : INotificationService
{
    private const int MAX_NOTIFICATIONS_PROCESSED_BY_CHUNK = 5;

    public void BulkAddNotificationForPublication(Guid publicationId)
    {
        var evnt = eventRepository.GetAll()
            .Include(x => x.Publication)
            .FirstOrDefault(x => x.Id == publicationId);
        NotFoundException<Event>.ThrowIfNull(evnt);

        var pub = evnt!.Publication;
        NotFoundException<Publication>.ThrowIfNull(pub);

        var subscriptions = subscriptionRepository.GetAll()
                                .Where(x => 
                                    x.OrganizerId == pub.OrganizerId &&
                                    x.DeletedAt == null)
                                .OrderBy(x => x.CreatedAt)
                                .ToList();

        foreach (var subscription in subscriptions)
        {
            notificationRepository.Add(new Notification
            {
                SubscriptionId = subscription.Id,
                PublicationId = pub.Id,
                IsSent = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        CleanUpNotifications();
    }

    private void CleanUpNotifications()
    {
        var notifications = notificationRepository.GetAll()
            .Where(x => x.DeletedAt == null && x.IsSent)
            .ToList();

        foreach (var notification in notifications)
        {
            notificationRepository.Delete(notification);
        }
    }

    public async Task<int> SendNewsForRemainingPublication()
    {
        var notifications = notificationRepository.GetAll()
            .Include(x => x.Publication)
                .ThenInclude(x => x.Organizer)
            .Include(x => x.Subscription)
            .Where(x => !x.IsSent && x.DeletedAt == null)
            .OrderBy(x => x.CreatedAt)
            .Take(MAX_NOTIFICATIONS_PROCESSED_BY_CHUNK)
            .ToList();

        if (notifications.Count == 0)
        {
            logger.LogInformation("No notification to process");
            return 0;
        }

        // send email to subscription.Email with subject and content
        var frontBaseUrl = Environment.GetEnvironmentVariable("FRONTEND_BASE_URL") ?? throw new Exception("FRONTEND_BASE_URL is not set");
        
        foreach (var notification in notifications)
        {
            var organizer = notification.Publication.Organizer;
            var buttonUrl = new Uri($"{frontBaseUrl}/fr/dashboard/profile/{organizer.Id}");
            var unsubscribeUrl = new Uri($"{frontBaseUrl}/fr/dashboard/unsubscribe/{notification.Subscription.SubscriptionToken}");

            await emailService.SendEmailAsync<object>(
                notification.Subscription.Email,
                "Nouvelle publication de {organizer.Organization}",
                new NotifyModel
                {
                    Title = $"Nouvelle publication de {organizer.Organization}",
                    Salutation = $"Bonjour!",
                    HeaderText = "Une nouvelle publication a été publiée par {organizer.Organization}! Voici un aperçu de l'annonce ",
                    PublicationTitle = notification.Publication.Title! ?? "",
                    ButtonSeePublicationText = "Voir la publication",
                    ButtonLink = buttonUrl,
                    UnsubscribeHeaderText = "Vous ne voulez plus recevoir de notifications?",
                    UnsubscribeLinkText = "Se désabonner",
                    UnsubscribeLink = unsubscribeUrl
                },
                EmailsUtils.NotifyTemplate);

            notification.IsSent = true;
            notification.UpdatedAt = DateTime.UtcNow;
            notificationRepository.Update(notification.Id, notification);
        }

        return notifications.Count;
    }

}
