using api.core.data.entities;
using api.core.Data.Enums;
using api.core.Data.Exceptions;
using api.core.Data.requests;
using api.core.repositories.abstractions;
using api.core.services.abstractions;
using api.core.Services;
using api.core.Services.Abstractions;
using api.emails.Models;
using api.emails.Services.Abstractions;
using api.files.Services.Abstractions;

using Microsoft.Extensions.Configuration;

using Moq;

namespace api.tests.Tests.Services;

public class EventServiceTests 
{

    private static readonly Guid TagId = Guid.NewGuid();

    private static readonly Guid ActivityAreaClubId = Guid.NewGuid();
    private static readonly Guid ActivityAreaSchoolId = Guid.NewGuid();

    private readonly List<Event> _events =
    [
        new Event
        {
            Id = Guid.NewGuid(),
            EventStartDate = DateTime.Now.AddDays(5),
            EventEndDate = DateTime.Now.AddDays(5).AddHours(1),
            Publication = new Publication
            {
                Title = "EVENT IN 5 DAYS",
                Content = "Test",
                ImageUrl = "http://example.com",
                State = State.Published,
                PublicationDate = DateTime.UtcNow,
                Tags =
                [
                    new Tag
                    {
                        Id = TagId,
                        Name = "Test"
                    }
                ],
                Organizer = new Organizer
                {
                    Id = Guid.NewGuid(),
                    ActivityAreaId = ActivityAreaClubId,
                    ActivityArea = new ActivityArea
                    {
                        Id = ActivityAreaClubId,
                        NameEn = "Club",
                        NameFr = "Club"
                    }
                },
                Moderator = new Moderator
                {
                    Id = Guid.NewGuid(),
                },
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }
        },
        new Event
        {
            Id = Guid.NewGuid(),
            EventStartDate = DateTime.Now.AddDays(1),
            EventEndDate = DateTime.Now.AddDays(1).AddHours(1),
            Publication = new Publication
            {
                Title = "EVENT TOMORROW, DIFFERENT ACTIVITY AREA",
                Content = "Test",
                ImageUrl = "http://example.com",
                State = State.Published,
                PublicationDate = DateTime.UtcNow,
                Tags =
                [
                    new Tag
                    {
                        Id = TagId,
                        Name = "Test"
                    }
                ],
                Organizer = new Organizer
                {
                    Id = Guid.NewGuid(),
                    ActivityAreaId = ActivityAreaSchoolId,
                    ActivityArea = new ActivityArea
                    {
                        Id = ActivityAreaSchoolId,
                        NameEn = "School",
                        NameFr = "School"
                    }
                },
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }
        },
        new Event
        {
            Id = Guid.NewGuid(),
            EventStartDate = DateTime.Now.AddDays(1),
            EventEndDate = DateTime.Now.AddDays(1).AddHours(1),
            Publication = new Publication
            {
                Title = "EVENT TOMORROW, WITHOUT TAGS",
                Content = "Test",
                ImageUrl = "http://example.com",
                State = State.Published,
                PublicationDate = DateTime.UtcNow,
                Tags = [],
                Organizer = new Organizer
                {
                    Id = Guid.NewGuid(),
                    ActivityAreaId = ActivityAreaSchoolId,
                    ActivityArea = new ActivityArea
                    {
                        Id = ActivityAreaSchoolId,
                        NameEn = "School",
                        NameFr = "School"
                    }
                },
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }
        },
        new Event
        {
            Id = Guid.NewGuid(),
            EventStartDate = DateTime.Now.AddDays(2),
            EventEndDate = DateTime.Now.AddDays(2).AddHours(1),
            Publication = new Publication
            {
                Title = "DELETED EVENT",
                Content = "Test",
                ImageUrl = "http://example.com",
                State = State.Deleted,
                PublicationDate = DateTime.UtcNow.AddDays(-1),
                Tags =
                [
                    new Tag
                    {
                        Id = TagId,
                        Name = "Test"
                    }
                ],
                Organizer = new Organizer
                {
                    Id = Guid.NewGuid(),
                    ActivityAreaId = ActivityAreaSchoolId,
                    ActivityArea = new ActivityArea
                    {
                        Id = ActivityAreaSchoolId,
                        NameEn = "School",
                        NameFr = "School"
                    }
                },
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                DeletedAt = DateTime.Now
            }
        }
    ];
    private EventService _eventService;

    private readonly Mock<IEventRepository> _mockEventRepository;
    private readonly Mock<ITagRepository> _mockTagRepository;
    private readonly Mock<IOrganizerRepository> _mockOrganizerRepository;
    private readonly Mock<IModeratorRepository> _mockModeratorRepository;
    private readonly Mock<IFileShareService> _mockFileShareService;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly Mock<INotificationService> _mockNotificationService;

    public EventServiceTests()
    {
        _mockEventRepository = new Mock<IEventRepository>();
        _mockTagRepository = new Mock<ITagRepository>();
        _mockOrganizerRepository = new Mock<IOrganizerRepository>();
        _mockModeratorRepository = new Mock<IModeratorRepository>();
        _mockFileShareService = new Mock<IFileShareService>();
        _mockEmailService = new Mock<IEmailService>();
        _mockNotificationService = new Mock<INotificationService>();

        _eventService = new EventService(
            _mockEventRepository.Object,
            _mockTagRepository.Object,
            _mockOrganizerRepository.Object,
            _mockModeratorRepository.Object,
            _mockFileShareService.Object,
            _mockEmailService.Object,
            _mockNotificationService.Object
            );
    }

    [Fact]
    public void GetEvents_ShouldReturnAllEventsExceptDeletedOnes()
    {
        // Arrange
        _mockEventRepository.Setup(repo => repo.GetAll()).Returns(_events.AsQueryable());

        // Act
        var events = _eventService.GetEvents(null, null, null, null, null, null, State.Published);

        // Assert
        _mockEventRepository.Verify(repo => repo.GetAll(), Times.Once);
        events.Should().NotBeEmpty();
        events.Should().HaveCount(3);
    }

    [Fact]
    public void GetEvents_ShouldReturnOnlyEventsAfterStartDate()
    {
        // Arrange
        _mockEventRepository.Setup(repo => repo.GetAll()).Returns(_events.AsQueryable());

        // Act
        var events = _eventService.GetEvents(DateTime.Now.AddDays(2), null, null, null, null, null, State.All);

        // Assert
        _mockEventRepository.Verify(repo => repo.GetAll(), Times.Once);
        events.Should().NotBeEmpty();
        events.Should().HaveCount(1);
    }

    [Fact]
    public void GetEvents_ShouldReturnOnlyEventsBeforeEndDate()
    {
        // Arrange

        _mockEventRepository.Setup(repo => repo.GetAll()).Returns(_events.AsQueryable());

        // Act
        var events = _eventService.GetEvents(null, DateTime.Now.AddDays(3), null, null, null, null, State.All);

        // Assert
        _mockEventRepository.Verify(repo => repo.GetAll(), Times.Once);
        events.Should().NotBeEmpty();
        events.Should().HaveCount(2);
    }

    [Fact]
    public void GetEvents_ShouldReturnOnlyEventsOnlyActivityAreaClub()
    {
        // Arrange
        _mockEventRepository.Setup(repo => repo.GetAll()).Returns(_events.AsQueryable());

        // Act
        var events = _eventService.GetEvents(null, null,
        [
            ActivityAreaClubId
        ], null, null, null, State.All, ignorePublicationDate: true);

        // Assert
        _mockEventRepository.Verify(repo => repo.GetAll(), Times.Once);
        events.Should().NotBeEmpty();
        events.Should().HaveCount(1);
    }

    [Fact]
    public void GetEvents_ShouldReturnOnlyEventsOnlyTagTest()
    {
        // Arrange
        _mockEventRepository.Setup(repo => repo.GetAll()).Returns(_events.AsQueryable());

        // Act
        var events = _eventService.GetEvents(null, null, null,
        [
            TagId
        ], null, null, State.All);

        // Assert
        _mockEventRepository.Verify(repo => repo.GetAll(), Times.Once);
        events.Should().NotBeEmpty();
        events.Should().HaveCount(2);
    }

    [Fact]
    public void GetEvents_ShouldReturnOnlyEventsWithMatchingTitle()
    {
        // Arrange
        var titleToMatch = "EVENT IN 5 DAYS";
        _mockEventRepository.Setup(repo => repo.GetAll()).Returns(_events.AsQueryable());

        // Act
        var events = _eventService.GetEvents(null, null, null, null, null, titleToMatch, State.All);

        // Assert
        _mockEventRepository.Verify(repo => repo.GetAll(), Times.Once);
        events.Should().NotBeEmpty();
        events.Should().HaveCount(1);
        events.Should().OnlyContain(e => e.Title!.Contains(titleToMatch));
    }


    [Fact]
    public void GetEvent_ShouldThrowAnException()
    {
        // Arrange
        _mockEventRepository.Setup(repo => repo.Get(_events.First().Id)).Returns((Event?)null);

        // Act
        _eventService.Invoking(s =>
            s.GetEvent(_events.First().Id))
                .Should().Throw<NotFoundException<Event>>();
    }


    [Fact]
    public void GetEvent_ShouldReturnEvent()
    {
        // Arrange
        _mockEventRepository.Setup(repo => repo.Get(_events.First().Id)).Returns(_events.First());

        // Act
        var evnt = _eventService.GetEvent(_events.First().Id);

        // Assert
        _mockEventRepository.Verify(repo => repo.Get(It.IsAny<Guid>()), Times.Once);
        evnt.Should().NotBeNull();
    }


    [Fact]
    public void AddEvents_ShouldThrowAnExceptionWhenOrganizerIsUnknown()
    {
        // Arrange
        _mockOrganizerRepository.Setup(repo => repo.Get(It.IsAny<Guid>())).Returns((Organizer?)null);

        // Act
        _eventService.Invoking(s =>
            s.AddEvent(Guid.Empty, new EventCreationRequestDTO()))
                .Should().Throw<UnauthorizedException>();
    }

    [Fact]
    public void DeleteEvent_ShouldReturnTrue_WhenEventIsDeletedSuccessfully()
    {
        // Arrange
        var userId = _events.First().Publication.Organizer.Id;
        var eventId = _events.First().Id;

        _mockEventRepository.Setup(repo => repo.Get(eventId)).Returns(_events.First());
        _mockEventRepository.Setup(repo => repo.Delete(It.IsAny<Event>())).Returns(true);

        // Act
        var result = _eventService.DeleteEvent(userId, eventId);

        // Assert
        result.Should().BeTrue();
        _mockEventRepository.Verify(repo => repo.Delete(It.IsAny<Event>()), Times.Once);
    }

    [Fact]
    public void DeleteEvent_ShouldThrowNotFoundException_WhenEventDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var eventId = Guid.NewGuid();

        _mockEventRepository.Setup(repo => repo.Get(eventId)).Returns((Event?)null);

        // Act
        Action act = () => _eventService.DeleteEvent(userId, eventId);

        // Assert
        act.Should().Throw<NotFoundException<Event>>();
        _mockEventRepository.Verify(repo => repo.Get(eventId), Times.Once);
    }

    [Fact]
    public void DeleteEvent_ShouldThrowUnauthorizedException_WhenUserIsNotAuthorized()
    {
        // Arrange
        var unauthorizedUserId = Guid.NewGuid();
        var eventId = _events.First().Id;

        _mockEventRepository.Setup(repo => repo.Get(eventId)).Returns(_events.First());

        // Act
        Action act = () => _eventService.DeleteEvent(unauthorizedUserId, eventId);

        // Assert
        act.Should().Throw<UnauthorizedException>();
        _mockEventRepository.Verify(repo => repo.Get(eventId), Times.Once);
    }


    [Fact]
    public void UpdateEvent_ShouldReturnTrue_WhenEventIsUpdatedSuccessfully()
    {
        // Arrange
        var userId = _events.First().Publication.Organizer.Id;
        var eventId = _events.First().Id;

        var request = new EventUpdateRequestDTO
        {
            Title = "Sample Event Title",
            Content = "This is a detailed description of the event.",
            Image = null,
            PublicationDate = DateTime.UtcNow,
            EventStartDate = DateTime.UtcNow.AddDays(10),
            EventEndDate = DateTime.UtcNow.AddDays(10).AddHours(1),
            Tags =
            [
                Guid.NewGuid(),
                Guid.NewGuid()
            ]
        };

        _mockEventRepository.Setup(repo => repo.Get(eventId)).Returns(_events.First());
        _mockEventRepository.Setup(repo => repo.Update(eventId, It.IsAny<Event>())).Returns(true);
        _mockOrganizerRepository.Setup(repo => repo.Get(It.IsAny<Guid>())).Returns(new Organizer { Id = userId });

        _eventService = new EventService(
            _mockEventRepository.Object,
            _mockTagRepository.Object,
            _mockOrganizerRepository.Object,
            _mockModeratorRepository.Object,
            _mockFileShareService.Object,
            _mockEmailService.Object,
            _mockNotificationService.Object);

        // Act
        var result = _eventService.UpdateEvent(userId, eventId, request);

        // Assert
        result.Should().BeTrue();
        _mockEventRepository.Verify(repo => repo.Update(eventId, It.IsAny<Event>()), Times.Once);
    }

    [Fact]
    public void UpdateEvent_ShouldThrowUnauthorizedException_WhenUserIsNotAuthorized()
    {
        // Arrange
        var unauthorizedUserId = Guid.NewGuid();
        var eventId = _events.First().Id;

        var request = new EventUpdateRequestDTO
        {
            Title = "Sample Event Title",
            Content = "This is a detailed description of the event.",
            PublicationDate = DateTime.UtcNow,
            EventStartDate = DateTime.UtcNow.AddDays(10),
            EventEndDate = DateTime.UtcNow.AddDays(10).AddHours(1),
            Tags =
            [
                Guid.NewGuid(),
                Guid.NewGuid()
            ]
        };

        // Assuming _events.First() returns an event where the organizer ID does not match `unauthorizedUserId`
        _mockEventRepository.Setup(repo => repo.Get(eventId)).Returns(_events.First());


        // Act
        Action act = () => _eventService.UpdateEvent(unauthorizedUserId, eventId, request);

        // Assert
        act.Should().Throw<UnauthorizedException>("because the user attempting to update the event does not have the proper permissions");
    }


    [Fact]
    public void UpdateEventState_ShouldReturnTrue_WhenStateIsUpdatedSuccessfullyByModerator()
    {
        // Arrange
        var userId = _events.First().Publication!.Moderator!.Id;
        var eventId = _events.First().Id;

        var newState = State.Approved;

        var eventToUpdate = _events.First();
        eventToUpdate.Publication.ModeratorId = userId;

        _mockEventRepository.Setup(repo => repo.Get(eventId)).Returns(eventToUpdate);
        _mockEventRepository.Setup(repo => repo.Update(eventId, It.IsAny<Event>())).Returns(true);
        _mockModeratorRepository.Setup(repo => repo.Get(It.IsAny<Guid>())).Returns(new Moderator { Id = userId });
        _mockEmailService.Setup(service => service.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<StatusChangeModel>(), It.IsAny<string>()));
        
        // Act
        var result = _eventService.UpdateEventState(userId, eventId, newState, null);

        // Assert
        result.Should().BeTrue();
        _mockEmailService.Verify(service => service.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<StatusChangeModel>(), It.IsAny<string>()), Times.Once);
        _mockEventRepository.Verify(repo => repo.Update(eventId, It.IsAny<Event>()), Times.Once);
    }

    [Fact]
    public void UpdateEventState_ShouldHaveStatePublished_WhenStateIsUpdatedWithApprovedAndPassedPublicationDate()
    {
        // Arrange
        var userId = _events.First().Publication!.Moderator!.Id;
        var eventId = _events.Last().Id;

        var newState = State.Approved;

        var eventToUpdate = _events.Last();

        _mockEventRepository.Setup(repo => repo.Get(eventId)).Returns(eventToUpdate);
        _mockEventRepository.Setup(repo => repo.Update(eventId, It.IsAny<Event>())).Returns(true);
        _mockModeratorRepository.Setup(repo => repo.Get(It.IsAny<Guid>())).Returns(new Moderator { Id = userId });
        _mockEmailService.Setup(service => service.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<StatusChangeModel>(), It.IsAny<string>()));

        // Act
        var result = _eventService.UpdateEventState(userId, eventId, newState, null);

        // Assert
        result.Should().BeTrue();
        eventToUpdate.Should().NotBeNull();
        eventToUpdate.Publication.State.Should().Be(State.Published);
        _mockEmailService.Verify(service => service.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<StatusChangeModel>(), It.IsAny<string>()), Times.Once);
        _mockEventRepository.Verify(repo => repo.Update(eventId, It.IsAny<Event>()), Times.Once);
    }


    [Fact]
    public void UpdateEventState_ShouldThrowUnauthorizedException_WhenUserIsNotAuthorized()
    {
        // Arrange
        var unauthorizedUserId = Guid.NewGuid();
        var eventId = _events.First().Id;
        var newState = State.Approved;

        var eventToUpdate = _events.First();
        eventToUpdate.Publication.ModeratorId = Guid.NewGuid();

        _mockEventRepository.Setup(repo => repo.Get(eventId)).Returns(eventToUpdate);

        // Act
        Action act = () => _eventService.UpdateEventState(unauthorizedUserId, eventId, newState, null);

        // Assert
        act.Should().Throw<UnauthorizedException>();
        _mockEmailService.Verify(service => service.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<StatusChangeModel>(), It.IsAny<string>()), Times.Never);
    }

}
