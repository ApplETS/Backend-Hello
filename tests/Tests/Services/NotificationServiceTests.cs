using api.core.repositories.abstractions;
using api.core.Services.Abstractions;
using api.core.Services;
using api.emails.Services.Abstractions;
using Moq;
using Microsoft.Extensions.Logging;
using api.core.data.entities;
using api.core.Data.Exceptions;
using api.emails.Models;
using Microsoft.Extensions.Configuration;
using api.emails;

namespace api.tests.Tests.Services;

public class NotificationServiceTests
{
    private NotificationService _notifService;

    private readonly Mock<ILogger<NotificationService>> _mockLogger;
    private readonly Mock<IEventRepository> _mockEventRepository;
    private readonly Mock<ISubscriptionRepository> _mockSubscriptionRepository;
    private readonly Mock<INotificationRepository> _mockNotifRepository;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly IConfiguration _config;

    public NotificationServiceTests()
    {
        _mockLogger = new Mock<ILogger<NotificationService>>();
        _mockEventRepository = new Mock<IEventRepository>();
        _mockSubscriptionRepository = new Mock<ISubscriptionRepository>();
        _mockNotifRepository = new Mock<INotificationRepository>();
        _mockEmailService = new Mock<IEmailService>();
        var data = new Dictionary<string, string>
        {
            {"FRONTEND_BASE_URL", "http://test.com"}
        };
        _config = new ConfigurationBuilder()
            .AddInMemoryCollection(data)
            .Build();

        _notifService = new NotificationService(
            _mockLogger.Object,
            _mockEventRepository.Object,
            _mockSubscriptionRepository.Object,
            _mockNotifRepository.Object,
            _mockEmailService.Object,
            _config
            );
    }

    [Fact]
    public void BulkAddNotificationForPublication_ShouldAdd2NotifsForPub()
    {
        // Arrange
        var pubId = Guid.NewGuid();
        var orgId = Guid.NewGuid();
        _mockEventRepository.Setup(x => x.GetAll()).Returns(new List<Event>
        {
            new Event
            {
                Id = pubId,
                Publication = new Publication
                {
                    Id = pubId,
                    OrganizerId = orgId
                }
            }
        }.AsQueryable());
        _mockSubscriptionRepository.Setup(x => x.GetAll()).Returns(new List<Subscription>
        {
            new Subscription
            {
                Id = Guid.NewGuid(),
                OrganizerId = orgId,
                DeletedAt = null,
                CreatedAt = DateTime.UtcNow
            },
            new Subscription
            {
                Id = Guid.NewGuid(),
                OrganizerId = orgId,
                DeletedAt = null,
                CreatedAt = DateTime.UtcNow
            }
        }.AsQueryable());
        _mockNotifRepository.Setup(x => x.Add(It.IsAny<Notification>())).Verifiable();

        // Act
        _notifService.BulkAddNotificationForPublication(pubId);

        // Assert
        _mockNotifRepository.Verify(x => x.Add(It.IsAny<Notification>()), Times.Exactly(2));
    }

    [Fact]
    public void BulkAddNotificationForPublication_ShouldThrowIfInvalidEvent()
    {
        // Arrange
        var pubId = Guid.NewGuid();
        _mockEventRepository.Setup(x => x.GetAll()).Returns(new List<Event>().AsQueryable());

        // Act
        _notifService
            .Invoking(n => n.BulkAddNotificationForPublication(pubId))
            .Should().Throw<NotFoundException<Event>>();
    }

    [Fact]
    public void BulkAddNotificationForPublication_ShouldThrowIfInvalidPublication()
    {
        // Arrange
        var pubId = Guid.NewGuid();
        var orgId = Guid.NewGuid();
        _mockEventRepository.Setup(x => x.GetAll()).Returns(new List<Event>
        {
            new Event
            {
                Id = pubId,
            }
        }.AsQueryable());

        // Act
        _notifService
            .Invoking(n => n.BulkAddNotificationForPublication(pubId))
            .Should().Throw<NotFoundException<Publication>>();
    }


    [Fact]
    public void BulkAddNotificationForPublication_ShouldAddNoneWhenSubscriptionOnOtherOrganizer()
    {
        // Arrange
        var pubId = Guid.NewGuid();
        var orgId = Guid.NewGuid();
        var otherOrgId = Guid.NewGuid();
        _mockEventRepository.Setup(x => x.GetAll()).Returns(new List<Event>
        {
            new Event
            {
                Id = pubId,
                Publication = new Publication
                {
                    Id = pubId,
                    OrganizerId = orgId
                }
            }
        }.AsQueryable());
        _mockSubscriptionRepository.Setup(x => x.GetAll()).Returns(new List<Subscription>
        {
            new Subscription
            {
                Id = Guid.NewGuid(),
                OrganizerId = otherOrgId,
                DeletedAt = null,
                CreatedAt = DateTime.UtcNow
            },
            new Subscription
            {
                Id = Guid.NewGuid(),
                OrganizerId = otherOrgId,
                DeletedAt = null,
                CreatedAt = DateTime.UtcNow
            }
        }.AsQueryable());
        _mockNotifRepository.Setup(x => x.Add(It.IsAny<Notification>())).Verifiable();

        // Act
        _notifService.BulkAddNotificationForPublication(pubId);

        // Assert
        _mockNotifRepository.Verify(x => x.Add(It.IsAny<Notification>()), Times.Never);
    }

    [Fact]
    public void BulkAddNotificationForPublication_ShouldCleanIsSentNotif()
    {
        // Arrange
        var pubId = Guid.NewGuid();
        var orgId = Guid.NewGuid();
        _mockEventRepository.Setup(x => x.GetAll()).Returns(new List<Event>
        {
            new Event
            {
                Id = pubId,
                Publication = new Publication
                {
                    Id = pubId,
                    OrganizerId = orgId
                }
            }
        }.AsQueryable());
        _mockSubscriptionRepository.Setup(x => x.GetAll()).Returns(new List<Subscription>
        {
            new Subscription
            {
                Id = Guid.NewGuid(),
                OrganizerId = orgId,
                DeletedAt = null,
                CreatedAt = DateTime.UtcNow
            }
        }.AsQueryable());
        _mockNotifRepository.Setup(x => x.GetAll()).Returns(new List<Notification>
        {
            new Notification
            {
                Id = Guid.NewGuid(),
                SubscriptionId = orgId,
                PublicationId = pubId,
                IsSent = true,
                DeletedAt = null,
                CreatedAt = DateTime.UtcNow
            }
        }.AsQueryable());
        _mockNotifRepository.Setup(x => x.Add(It.IsAny<Notification>())).Verifiable();
        _mockNotifRepository.Setup(x => x.Delete(It.IsAny<Notification>())).Verifiable();

        // Act
        _notifService.BulkAddNotificationForPublication(pubId);

        // Assert
        _mockNotifRepository.Verify(x => x.Add(It.IsAny<Notification>()), Times.Once);
        _mockNotifRepository.Verify(x => x.Delete(It.IsAny<Notification>()), Times.Once);
    }

    [Fact]
    public async Task SendNewsForRemainingPublication_ShouldSendNotificationWaiting()
    {
        // Arrange
        var notifId = Guid.NewGuid();
        var pubId = Guid.NewGuid();
        var orgId = Guid.NewGuid();
        _mockNotifRepository.Setup(x => x.GetAll()).Returns(new List<Notification>
        {
            new Notification
            {
                Id = notifId,
                SubscriptionId = orgId,
                Subscription = new Subscription
                {
                    Id = orgId,
                    OrganizerId = orgId,
                    SubscriptionToken = "token"
                },
                PublicationId = pubId,
                Publication = new Publication
                {
                    Id = pubId,
                    OrganizerId = orgId,
                    Organizer = new Organizer
                    {
                        Id = orgId,
                        Email = "blabla@bla.com",
                    }
                },
                IsSent = false,
                DeletedAt = null,
                CreatedAt = DateTime.UtcNow
            }
        }.AsQueryable());

        _mockEmailService.Setup(x => x.SendEmailAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    EmailsUtils.NotifyTemplate)).Verifiable();
        _mockNotifRepository.Setup(x => x.Update(notifId, It.IsAny<Notification>())).Verifiable();

        // Act
        var notif = await _notifService.SendNewsForRemainingPublication();

        // Assert
        _mockEmailService.Verify(x => x.SendEmailAsync(
                       It.IsAny<string>(),
                       It.IsAny<string>(),
                       It.IsAny<object>(),
                       It.IsAny<string>()), Times.Once);
        _mockNotifRepository.Verify(_mockNotifRepository => _mockNotifRepository.Update(notifId, It.IsAny<Notification>()), Times.Once);

        notif.Should().Be(1);
    }

    [Fact]
    public async Task SendNewsForRemainingPublication_ShouldReturn0Early()
    {
        // Arrange
        var pubId = Guid.NewGuid();
        var orgId = Guid.NewGuid();
        _mockNotifRepository.Setup(x => x.GetAll()).Returns(new List<Notification>
        {
            new Notification
            {
                Id = Guid.NewGuid(),
                SubscriptionId = orgId,
                Subscription = new Subscription
                {
                    Id = orgId,
                    OrganizerId = orgId,
                    SubscriptionToken = "token"
                },
                PublicationId = pubId,
                Publication = new Publication
                {
                    Id = pubId,
                    OrganizerId = orgId,
                    Organizer = new Organizer
                    {
                        Id = orgId,
                        Email = "blabla@bla.com",
                    }
                },
                IsSent = true, // Won't be sent twice
                DeletedAt = null,
                CreatedAt = DateTime.UtcNow
            }
        }.AsQueryable());
        _mockEmailService.Setup(x => x.SendEmailAsync(
                       It.IsAny<string>(),
                       It.IsAny<string>(),
                       It.IsAny<NotifyModel>(),
                       It.IsAny<string>())).Verifiable();

        // Act
        var notif = await _notifService.SendNewsForRemainingPublication();

        // Assert
        _mockEmailService.Verify(x => x.SendEmailAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<NotifyModel>(),
            It.IsAny<string>()), Times.Never);
        notif.Should().Be(0);
    }
}
