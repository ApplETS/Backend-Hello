using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using api.core.data.entities;
using api.core.Data.Exceptions;
using api.core.Data.requests;
using api.core.repositories.abstractions;
using api.core.Repositories.Abstractions;
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
            EventDate = DateTime.Now.AddDays(5),
            Publication = new Publication
            {
                Title = "EVENT IN 5 DAYS",
                Content = "Test",
                ImageUrl = "Test",
                State = "Test",
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
                    Name = "Test",
                    ActivityArea = "Club"
                },
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }
        },
        new Event
        {
            Id = Guid.NewGuid(),
            EventDate = DateTime.Now.AddDays(1),
            Publication = new Publication
            {
                Title = "EVENT TOMORROW, DIFFERENT ACTIVITY AREA",
                Content = "Test",
                ImageUrl = "Test",
                State = "Test",
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
                    Name = "Test",
                    ActivityArea = "School"
                },
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }
        },
        new Event
        {
            Id = Guid.NewGuid(),
            EventDate = DateTime.Now.AddDays(1),
            Publication = new Publication
            {
                Title = "EVENT TOMORROW, WITHOUT TAGS",
                Content = "Test",
                ImageUrl = "Test",
                State = "Test",
                PublicationDate = DateTime.Now,
                Tags = new List<Tag>(),
                Organizer = new Organizer
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    ActivityArea = "School"
                },
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }
        },
        new Event
        {
            Id = Guid.NewGuid(),
            EventDate = DateTime.Now.AddDays(2),
            Publication = new Publication
            {
                Title = "DELETED EVENT",
                Content = "Test",
                ImageUrl = "Test",
                State = "Test",
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
                    Name = "Test",
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

        var eventService = new EventService(mockEventRepository.Object, mockTagRepository.Object, mockOrganizerRepository.Object);
        
        mockEventRepository.Setup(repo => repo.GetAll()).Returns(_events);

        // Act
        var events = eventService.GetEvents(null, null, null, null);

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

        var eventService = new EventService(mockEventRepository.Object, mockTagRepository.Object, mockOrganizerRepository.Object);

        mockEventRepository.Setup(repo => repo.GetAll()).Returns(_events);

        // Act
        var events = eventService.GetEvents(DateTime.Now.AddDays(2), null, null, null);

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

        var eventService = new EventService(mockEventRepository.Object, mockTagRepository.Object, mockOrganizerRepository.Object);

        mockEventRepository.Setup(repo => repo.GetAll()).Returns(_events);

        // Act
        var events = eventService.GetEvents(null, DateTime.Now.AddDays(3), null, null);

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

        var eventService = new EventService(mockEventRepository.Object, mockTagRepository.Object, mockOrganizerRepository.Object);

        mockEventRepository.Setup(repo => repo.GetAll()).Returns(_events);

        // Act
        var events = eventService.GetEvents(null, null, new List<string>
        {
            "Club"
        }, null);

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

        var eventService = new EventService(mockEventRepository.Object, mockTagRepository.Object, mockOrganizerRepository.Object);

        mockEventRepository.Setup(repo => repo.GetAll()).Returns(_events);

        // Act
        var events = eventService.GetEvents(null, null, null, new List<Guid>
        {
            _tagId
        });

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

        var eventService = new EventService(mockEventRepository.Object, mockTagRepository.Object, mockOrganizerRepository.Object);

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

        var eventService = new EventService(mockEventRepository.Object, mockTagRepository.Object, mockOrganizerRepository.Object);
        

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

        mockOrganizerRepository.Setup(repo => repo.Get(It.IsAny<Guid>())).Returns((Organizer?)null);

        var eventService = new EventService(mockEventRepository.Object, mockTagRepository.Object, mockOrganizerRepository.Object);

        // Act
        eventService.Invoking(s =>
            s.AddEvent(Guid.Empty, new EventRequestDTO()))
                .Should().Throw<UnauthorizedException>();
    }
}
