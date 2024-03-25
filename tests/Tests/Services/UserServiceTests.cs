using Xunit;
using Moq;
using FluentAssertions;
using System;
using api.core.data.entities;
using api.core.Data.requests;
using api.core.repositories.abstractions;
using api.core.Services;
using api.core.Data.Responses;
using api.files.Services.Abstractions;

namespace api.tests.Tests.Services;
public class UserServiceTests
{
    private readonly Mock<IOrganizerRepository> _organizerRepositoryMock;
    private readonly Mock<IModeratorRepository> _moderatorRepositoryMock;
    private readonly Mock<IFileShareService> _fileShareServiceMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _organizerRepositoryMock = new Mock<IOrganizerRepository>();
        _moderatorRepositoryMock = new Mock<IModeratorRepository>();
        _fileShareServiceMock = new Mock<IFileShareService>();
        _fileShareServiceMock.Setup(service => service.FileGetDownloadUri(It.IsAny<string>())).Returns(new Uri("http://example.com/avatar.webp"));
        _userService = new UserService(_organizerRepositoryMock.Object, _fileShareServiceMock.Object, _moderatorRepositoryMock.Object);
    }

    [Fact]
    public void AddOrganizer_ShouldReturnUserResponseDTO_WhenOrganizerIsAddedSuccessfully()
    {
        // Arrange
        var organizerDto = new UserUpdateDTO
        {
            Email = "john.doe@example.com",
            Organization = "ExampleOrg",
            ActivityArea = "Tech"
        };

        var organizer = new Organizer
        {
            Email = organizerDto.Email,
            Organization = organizerDto.Organization,
            ActivityArea = organizerDto.ActivityArea,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _organizerRepositoryMock.Setup(repo => repo.Add(It.IsAny<Organizer>())).Returns(organizer);

        // Act
        var result = _userService.AddOrganizer(Guid.NewGuid(), organizerDto);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(organizerDto.Email);
        result.Organization.Should().Be(organizerDto.Organization);
        result.ActivityArea.Should().Be(organizerDto.ActivityArea);

        _organizerRepositoryMock.Verify(repo => repo.Add(It.IsAny<Organizer>()), Times.Once);
    }

    [Fact]
    public void GetUser_ShouldReturnUserResponseDTO_WhenOrganizerIsFoundById()
    {
        // Arrange
        var organizerId = Guid.NewGuid();
        var organizer = new Organizer
        {
            Id = organizerId,
            Email = "john.doe@example.com",
            Organization = "ExampleOrg",
            ActivityArea = "Tech",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _organizerRepositoryMock.Setup(repo => repo.Get(organizerId)).Returns(organizer);

        // Act
        var result = _userService.GetUser(organizerId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(organizerId);
        result.Email.Should().Be(organizer.Email);

        _organizerRepositoryMock.Verify(repo => repo.Get(organizerId), Times.Once);
    }

    [Fact]
    public void GetUser_ShouldReturnUserResponseDTO_WhenModeratorIsFoundById()
    {
        // Arrange
        var moderatorId = Guid.NewGuid();
        var moderator = new Moderator
        {
            Id = moderatorId,
            Email = "jane.doe@example.com",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _organizerRepositoryMock.Setup(repo => repo.Get(moderatorId)).Returns((Organizer?)null); // Simulate no organizer found
        _moderatorRepositoryMock.Setup(repo => repo.Get(moderatorId)).Returns(moderator); // Simulate moderator found

        // Act
        var result = _userService.GetUser(moderatorId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(moderatorId);
        result.Email.Should().Be(moderator.Email);

        _organizerRepositoryMock.Verify(repo => repo.Get(moderatorId), Times.Once);
        _moderatorRepositoryMock.Verify(repo => repo.Get(moderatorId), Times.Once);
    }

    [Fact]
    public void GetUser_ShouldThrowException_WhenNoUserIsAssociatedWithProvidedId()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Setup both organizer and moderator repositories to return null, simulating that no user is found with the provided ID
        _organizerRepositoryMock.Setup(repo => repo.Get(userId)).Returns(null as Organizer);
        _moderatorRepositoryMock.Setup(repo => repo.Get(userId)).Returns(null as Moderator);

        // Act
        Action act = () => _userService.GetUser(userId);

        // Assert
        act.Should().Throw<Exception>().WithMessage("No users associated with this ID");
        _organizerRepositoryMock.Verify(repo => repo.Get(userId), Times.Once);
        _moderatorRepositoryMock.Verify(repo => repo.Get(userId), Times.Once);
    }


    [Fact]
    public void UpdateUser_ShouldReturnTrue_WhenOrganizerIsUpdatedSuccessfully()
    {
        // Arrange
        var organizerId = Guid.NewGuid();
        var updateDto = new UserUpdateDTO
        {
            Email = "jane.doe@example.com",
            Organization = "NewOrg",
            ActivityArea = "Health"
        };

        var existingOrganizer = new Organizer
        {
            Id = organizerId,
            Email = "john.doe@example.com",
            Organization = "ExampleOrg",
            ActivityArea = "Tech",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _organizerRepositoryMock.Setup(repo => repo.Get(organizerId)).Returns(existingOrganizer);
        _organizerRepositoryMock.Setup(repo => repo.Update(organizerId, It.IsAny<Organizer>())).Returns(true);

        // Act
        var result = _userService.UpdateUser(organizerId, updateDto);

        // Assert
        result.Should().BeTrue();

        _organizerRepositoryMock.Verify(repo => repo.Update(organizerId, It.IsAny<Organizer>()), Times.Once);
    }

    [Fact]
    public void UpdateUser_ShouldReturnTrue_WhenModeratorIsUpdatedSuccessfully()
    {
        // Arrange
        var moderatorId = Guid.NewGuid();
        var updateDto = new UserUpdateDTO
        {
            Email = "john.updated@example.com"
        };

        var existingModerator = new Moderator
        {
            Id = moderatorId,
            Email = "john.doe@example.com",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _moderatorRepositoryMock.Setup(repo => repo.Get(moderatorId)).Returns(existingModerator); // Simulate moderator found
        _moderatorRepositoryMock.Setup(repo => repo.Update(moderatorId, It.IsAny<Moderator>())).Returns(true);

        // Act
        var result = _userService.UpdateUser(moderatorId, updateDto);

        // Assert
        result.Should().BeTrue();

        _moderatorRepositoryMock.Verify(repo => repo.Update(moderatorId, It.IsAny<Moderator>()), Times.Once);
    }
}
