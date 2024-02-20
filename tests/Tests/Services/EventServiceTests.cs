using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using api.core.data.entities;
using api.core.Data.Entities;
using api.core.Data.Exceptions;
using api.core.Data.requests;
using api.core.repositories.abstractions;
using api.core.Services;

using FluentAssertions;

using Moq;

namespace api.tests.Tests.Services;

public class EventServiceTests
{
    private static readonly Guid _tagId = Guid.NewGuid();

    private readonly List<Event> _events = new List<Event>
    {
        new Event
        {
            Id = Guid.NewGuid(),
            EventStartDate = DateTime.Now.AddDays(5),
            EventEndDate = DateTime.Now.AddDays(5).AddHours(1),
            Publication = new Publication
            {
                Title = "EVENT IN 5 DAYS",
                Content = "Test",
                ImageUrl = "Test",
                State = State.Published,
                PublicationDate = DateTime.Now,
                Tags = new List<Tag>
                {
                    new Tag
                    {
                        Id = _tagId,
                        Name = "Test"
                    }
                },
                Organizer = new Organizer
                {
                    Id = Guid.NewGuid(),
                    ActivityArea = "Club"
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
                ImageUrl = "Test",
                State = State.Published,
                PublicationDate = DateTime.Now,
                Tags = new List<Tag>
                {
                    new Tag
                    {
                        Id = _tagId,
                        Name = "Test"
                    }
                },
                Organizer = new Organizer
                {
                    Id = Guid.NewGuid(),
                    ActivityArea = "School"
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
                ImageUrl = "Test",
                State = State.Published,
                PublicationDate = DateTime.Now,
                Tags = new List<Tag>(),
                Organizer = new Organizer
                {
                    Id = Guid.NewGuid(),
                    ActivityArea = "School"
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
                ImageUrl = "Test",
                State = State.Deleted,
                PublicationDate = DateTime.Now,
                Tags = new List<Tag>
                {
                    new Tag
                    {
                        Id = _tagId,
                        Name = "Test"
                    }
                },
                Organizer = new Organizer
                {
                    Id = Guid.NewGuid(),
                    ActivityArea = "School"
                },
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                DeletedAt = DateTime.Now
            }
        }
    };

    [Fact]
    public void GetEvents_ShouldReturnAllEventsExceptDeletedOnes()
    {
        // Arrange
        var mockEventRepository = new Mock<IEventRepository>();
        var mockTagRepository = new Mock<ITagRepository>();
        var mockOrganizerRepository = new Mock<IOrganizerRepository>();
        var mockModeratorRepository = new Mock<IModeratorRepository>();

        var eventService = new EventService(mockEventRepository.Object, mockTagRepository.Object, mockOrganizerRepository.Object, mockModeratorRepository.Object);
        
        mockEventRepository.Setup(repo => repo.GetAll()).Returns(_events);

        // Act
        var events = eventService.GetEvents(null, null, null, null, null, State.Published);

        // Assert
        mockEventRepository.Verify(repo => repo.GetAll(), Times.Once);
        events.Should().NotBeEmpty();
        events.Should().HaveCount(3);
    }

    [Fact]
    public void GetEvents_ShouldReturnOnlyEventsAfterStartDate()
    {
        // Arrange
        var mockEventRepository = new Mock<IEventRepository>();
        var mockTagRepository = new Mock<ITagRepository>();
        var mockOrganizerRepository = new Mock<IOrganizerRepository>();
        var mockModeratorRepository = new Mock<IModeratorRepository>();

        var eventService = new EventService(mockEventRepository.Object, mockTagRepository.Object, mockOrganizerRepository.Object, mockModeratorRepository.Object);

        mockEventRepository.Setup(repo => repo.GetAll()).Returns(_events);

        // Act
        var events = eventService.GetEvents(DateTime.Now.AddDays(2), null, null, null, null, State.All);

        // Assert
        mockEventRepository.Verify(repo => repo.GetAll(), Times.Once);
        events.Should().NotBeEmpty();
        events.Should().HaveCount(1);
    }

    [Fact]
    public void GetEvents_ShouldReturnOnlyEventsBeforeEndDate()
    {
        // Arrange
        var mockEventRepository = new Mock<IEventRepository>();
        var mockTagRepository = new Mock<ITagRepository>();
        var mockOrganizerRepository = new Mock<IOrganizerRepository>();
        var mockModeratorRepository = new Mock<IModeratorRepository>();

        var eventService = new EventService(mockEventRepository.Object, mockTagRepository.Object, mockOrganizerRepository.Object, mockModeratorRepository.Object);

        mockEventRepository.Setup(repo => repo.GetAll()).Returns(_events);

        // Act
        var events = eventService.GetEvents(null, DateTime.Now.AddDays(3), null, null, null, State.All);

        // Assert
        mockEventRepository.Verify(repo => repo.GetAll(), Times.Once);
        events.Should().NotBeEmpty();
        events.Should().HaveCount(2);
    }

    [Fact]
    public void GetEvents_ShouldReturnOnlyEventsOnlyActivityAreaClub()
    {
        // Arrange
        var mockEventRepository = new Mock<IEventRepository>();
        var mockTagRepository = new Mock<ITagRepository>();
        var mockOrganizerRepository = new Mock<IOrganizerRepository>();
        var mockModeratorRepository = new Mock<IModeratorRepository>();

        var eventService = new EventService(mockEventRepository.Object, mockTagRepository.Object, mockOrganizerRepository.Object, mockModeratorRepository.Object);

        mockEventRepository.Setup(repo => repo.GetAll()).Returns(_events);

        // Act
        var events = eventService.GetEvents(null, null, new List<string>
        {
            "Club"
        }, null, null, State.All);

        // Assert
        mockEventRepository.Verify(repo => repo.GetAll(), Times.Once);
        events.Should().NotBeEmpty();
        events.Should().HaveCount(1);
    }

    [Fact]
    public void GetEvents_ShouldReturnOnlyEventsOnlyTagTest()
    {
        // Arrange
        var mockEventRepository = new Mock<IEventRepository>();
        var mockTagRepository = new Mock<ITagRepository>();
        var mockOrganizerRepository = new Mock<IOrganizerRepository>();
        var mockModeratorRepository = new Mock<IModeratorRepository>();

        var eventService = new EventService(mockEventRepository.Object, mockTagRepository.Object, mockOrganizerRepository.Object, mockModeratorRepository.Object);

        mockEventRepository.Setup(repo => repo.GetAll()).Returns(_events);

        // Act
        var events = eventService.GetEvents(null, null, null, new List<Guid>
        {
            _tagId
        }, null, State.All);

        // Assert
        mockEventRepository.Verify(repo => repo.GetAll(), Times.Once);
        events.Should().NotBeEmpty();
        events.Should().HaveCount(2);
    }


    [Fact]
    public void GetEvent_ShouldThrowAnException()
    {
        // Arrange
        var mockEventRepository = new Mock<IEventRepository>();
        var mockTagRepository = new Mock<ITagRepository>();
        var mockOrganizerRepository = new Mock<IOrganizerRepository>();
        var mockModeratorRepository = new Mock<IModeratorRepository>();

        var eventService = new EventService(mockEventRepository.Object, mockTagRepository.Object, mockOrganizerRepository.Object, mockModeratorRepository.Object);

        mockEventRepository.Setup(repo => repo.Get(_events.First().Id)).Returns((Event?)null);

        // Act
        eventService.Invoking(s =>
            s.GetEvent(_events.First().Id))
                .Should().Throw<NotFoundException<Event>>();
    }


    [Fact]
    public void GetEvent_ShouldReturnEvent()
    {
        // Arrange
        var mockEventRepository = new Mock<IEventRepository>();
        var mockTagRepository = new Mock<ITagRepository>();
        var mockOrganizerRepository = new Mock<IOrganizerRepository>();
        var mockModeratorRepository = new Mock<IModeratorRepository>();

        var eventService = new EventService(mockEventRepository.Object, mockTagRepository.Object, mockOrganizerRepository.Object, mockModeratorRepository.Object);


        mockEventRepository.Setup(repo => repo.Get(_events.First().Id)).Returns(_events.First());

        // Act
        var evnt = eventService.GetEvent(_events.First().Id);

        // Assert
        mockEventRepository.Verify(repo => repo.Get(It.IsAny<Guid>()), Times.Once);
        evnt.Should().NotBeNull();
    }


    [Fact]
    public void AddEvents_ShouldThrowAnExceptionWhenOrganizerIsUnknown()
    {
        // Arrange
        var mockEventRepository = new Mock<IEventRepository>();
        var mockTagRepository = new Mock<ITagRepository>();
        var mockOrganizerRepository = new Mock<IOrganizerRepository>();
        var mockModeratorRepository = new Mock<IModeratorRepository>();

        mockOrganizerRepository.Setup(repo => repo.Get(It.IsAny<Guid>())).Returns((Organizer?)null);

        var eventService = new EventService(mockEventRepository.Object, mockTagRepository.Object, mockOrganizerRepository.Object, mockModeratorRepository.Object);

        // Act
        eventService.Invoking(s =>
            s.AddEvent(Guid.Empty, new EventRequestDTO()))
                .Should().Throw<UnauthorizedException>();
    }

    [Fact]
    public void DeleteEvent_ShouldReturnTrue_WhenEventIsDeletedSuccessfully()
    {
        // Arrange
        var mockEventRepository = new Mock<IEventRepository>();
        var userId = _events.First().Publication.Organizer.Id;
        var eventId = _events.First().Id;

        mockEventRepository.Setup(repo => repo.Get(eventId)).Returns(_events.First());
        mockEventRepository.Setup(repo => repo.Delete(It.IsAny<Event>())).Returns(true);

        var eventService = new EventService(mockEventRepository.Object, new Mock<ITagRepository>().Object, new Mock<IOrganizerRepository>().Object, new Mock<IModeratorRepository>().Object);

        // Act
        var result = eventService.DeleteEvent(userId, eventId);

        // Assert
        result.Should().BeTrue();
        mockEventRepository.Verify(repo => repo.Delete(It.IsAny<Event>()), Times.Once);
    }

    [Fact]
    public void DeleteEvent_ShouldThrowNotFoundException_WhenEventDoesNotExist()
    {
        // Arrange
        var mockEventRepository = new Mock<IEventRepository>();
        var userId = Guid.NewGuid();
        var eventId = Guid.NewGuid();

        mockEventRepository.Setup(repo => repo.Get(eventId)).Returns((Event?)null);

        var eventService = new EventService(mockEventRepository.Object, new Mock<ITagRepository>().Object, new Mock<IOrganizerRepository>().Object, new Mock<IModeratorRepository>().Object);

        // Act
        Action act = () => eventService.DeleteEvent(userId, eventId);

        // Assert
        act.Should().Throw<NotFoundException<Event>>();
        mockEventRepository.Verify(repo => repo.Get(eventId), Times.Once);
    }

    [Fact]
    public void DeleteEvent_ShouldThrowUnauthorizedException_WhenUserIsNotAuthorized()
    {
        // Arrange
        var mockEventRepository = new Mock<IEventRepository>();
        var unauthorizedUserId = Guid.NewGuid();
        var eventId = _events.First().Id;

        mockEventRepository.Setup(repo => repo.Get(eventId)).Returns(_events.First());

        var eventService = new EventService(mockEventRepository.Object, new Mock<ITagRepository>().Object, new Mock<IOrganizerRepository>().Object, new Mock<IModeratorRepository>().Object);

        // Act
        Action act = () => eventService.DeleteEvent(unauthorizedUserId, eventId);

        // Assert
        act.Should().Throw<UnauthorizedException>();
        mockEventRepository.Verify(repo => repo.Get(eventId), Times.Once);
    }


    [Fact]
    public void UpdateEvent_ShouldReturnTrue_WhenEventIsUpdatedSuccessfully()
    {
        // Arrange
        var mockEventRepository = new Mock<IEventRepository>();
        var mockOrganizerRepository = new Mock<IOrganizerRepository>();
        var userId = _events.First().Publication.Organizer.Id;
        var eventId = _events.First().Id;

        var request = new EventRequestDTO
        {
            Id = Guid.NewGuid(),
            Title = "Sample Event Title",
            Content = "This is a detailed description of the event.",
            ImageUrl = "https://example.com/image.jpg",
            State = State.Approved,
            PublicationDate = DateTime.UtcNow,
            EventStartDate = DateTime.UtcNow.AddDays(10),
            EventEndDate = DateTime.UtcNow.AddDays(10).AddHours(1),
            Tags = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            }
        };

        mockEventRepository.Setup(repo => repo.Get(eventId)).Returns(_events.First());
        mockEventRepository.Setup(repo => repo.Update(eventId, It.IsAny<Event>())).Returns(true);
        mockOrganizerRepository.Setup(repo => repo.Get(It.IsAny<Guid>())).Returns(new Organizer { Id = userId });

        var eventService = new EventService(mockEventRepository.Object, new Mock<ITagRepository>().Object, mockOrganizerRepository.Object, new Mock<IModeratorRepository>().Object);

        // Act
        var result = eventService.UpdateEvent(userId, eventId, request);

        // Assert
        result.Should().BeTrue();
        mockEventRepository.Verify(repo => repo.Update(eventId, It.IsAny<Event>()), Times.Once);
    }

    [Fact]
    public void UpdateEvent_ShouldThrowUnauthorizedException_WhenUserIsNotAuthorized()
    {
        // Arrange
        var mockEventRepository = new Mock<IEventRepository>();
        var unauthorizedUserId = Guid.NewGuid();
        var eventId = _events.First().Id;

        var request = new EventRequestDTO
        {
            Id = Guid.NewGuid(),
            Title = "Sample Event Title",
            Content = "This is a detailed description of the event.",
            ImageUrl = "https://example.com/image.jpg",
            State = State.Approved,
            PublicationDate = DateTime.UtcNow,
            EventStartDate = DateTime.UtcNow.AddDays(10),
            EventEndDate = DateTime.UtcNow.AddDays(10).AddHours(1),
            Tags = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            }
        };

        // Assuming _events.First() returns an event where the organizer ID does not match `unauthorizedUserId`
        mockEventRepository.Setup(repo => repo.Get(eventId)).Returns(_events.First());

        var eventService = new EventService(mockEventRepository.Object, new Mock<ITagRepository>().Object, new Mock<IOrganizerRepository>().Object, new Mock<IModeratorRepository>().Object);

        // Act
        Action act = () => eventService.UpdateEvent(unauthorizedUserId, eventId, request);

        // Assert
        act.Should().Throw<UnauthorizedException>("because the user attempting to update the event does not have the proper permissions");
    }


    [Fact]
    public void UpdateEventState_ShouldReturnTrue_WhenStateIsUpdatedSuccessfullyByModerator()
    {
        // Arrange
        var mockEventRepository = new Mock<IEventRepository>();
        var mockModeratorRepository = new Mock<IModeratorRepository>();
        var userId = _events.First().Publication.Moderator.Id;
        var eventId = _events.First().Id;

        var newState = core.Data.Entities.State.Approved;

        var eventToUpdate = _events.First();
        eventToUpdate.Publication.ModeratorId = userId;

        mockEventRepository.Setup(repo => repo.Get(eventId)).Returns(eventToUpdate);
        mockEventRepository.Setup(repo => repo.Update(eventId, It.IsAny<Event>())).Returns(true);
        mockModeratorRepository.Setup(repo => repo.Get(It.IsAny<Guid>())).Returns(new Moderator { Id = userId });

        var eventService = new EventService(mockEventRepository.Object, new Mock<ITagRepository>().Object, new Mock<IOrganizerRepository>().Object, mockModeratorRepository.Object);

        // Act
        var result = eventService.UpdateEventState(userId, eventId, newState);

        // Assert
        result.Should().BeTrue();
        mockEventRepository.Verify(repo => repo.Update(eventId, It.IsAny<Event>()), Times.Once);
    }

    [Fact]
    public void UpdateEventState_ShouldThrowUnauthorizedException_WhenUserIsNotAuthorized()
    {
        // Arrange
        var mockEventRepository = new Mock<IEventRepository>();
        var unauthorizedUserId = Guid.NewGuid();
        var eventId = _events.First().Id;
        var newState = State.Approved;

        var eventToUpdate = _events.First();
        eventToUpdate.Publication.ModeratorId = Guid.NewGuid();

        mockEventRepository.Setup(repo => repo.Get(eventId)).Returns(eventToUpdate);

        var eventService = new EventService(mockEventRepository.Object, new Mock<ITagRepository>().Object, new Mock<IOrganizerRepository>().Object, new Mock<IModeratorRepository>().Object);

        // Act
        Action act = () => eventService.UpdateEventState(unauthorizedUserId, eventId, newState);

        // Assert
        act.Should().Throw<UnauthorizedException>();
    }

}
