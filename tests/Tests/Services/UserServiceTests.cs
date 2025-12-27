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
using System.Diagnostics;
using api.core.Data.Exceptions;
using api.core.services.abstractions;
using api.core.Data.Entities;
using api.core.Repositories.Abstractions;

namespace api.tests.Tests.Services;
public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IActivityAreaRepository> _activityAreaRepositoryMock;
    private readonly Mock<ITagRepository> _tagRepositoryMock;
    private readonly Mock<IFileShareService> _fileShareServiceMock;
    private readonly Mock<IImageService> _imageServiceMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _tagRepositoryMock = new Mock<ITagRepository>();
        _activityAreaRepositoryMock = new Mock<IActivityAreaRepository>();
        _fileShareServiceMock = new Mock<IFileShareService>();
        _imageServiceMock = new Mock<IImageService>();

        _fileShareServiceMock.Setup(service => service.FileGetDownloadUri(It.IsAny<string>())).Returns(new Uri("http://example.com/avatar.webp"));
        _userService = new UserService(
            _userRepositoryMock.Object,
            _fileShareServiceMock.Object,
            _tagRepositoryMock.Object,
            _activityAreaRepositoryMock.Object,
            _imageServiceMock.Object);
    }

    [Fact]
    public void AddOrganizer_ShouldReturnUserResponseDTO_WhenOrganizerIsAddedSuccessfully()
    {
        // Arrange
        var actAreaModified = Guid.NewGuid();
        var organizerDto = new UserUpdateDTO
        {
            Email = "john.doe@example.com",
            Organization = "ExampleOrg",
            ActivityAreaId = actAreaModified,
            Id = "1234"
        };

        var activity = new ActivityArea
        {
            Id = actAreaModified,
            NameFr = "Tech",
        };
        var organizer = new User
        {
            Email = organizerDto.Email,
            Organization = organizerDto.Organization,
            ActivityAreaId = actAreaModified,
            ActivityArea = activity,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _activityAreaRepositoryMock.Setup(repo => repo.Get(It.IsAny<Guid>())).Returns(activity);

        _userRepositoryMock.Setup(repo => repo.Add(It.IsAny<User>())).Returns(organizer);

        // Act
        var result = _userService.AddOrganizer("1234", organizerDto);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(organizerDto.Email);
        result.Organization.Should().Be(organizerDto.Organization);
        result.ActivityArea.Id.ToString().Should().Be(actAreaModified.ToString());

        _userRepositoryMock.Verify(repo => repo.Add(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public void GetUser_ShouldReturnUserResponseDTO_WhenOrganizerIsFoundById()
    {
        // Arrange
        var organizerId = "organizer";
        var organizer = new User
        {
            Id = organizerId,
            Email = "john.doe@example.com",
            Organization = "ExampleOrg",
            ActivityArea = new ActivityArea
            {
                Id = Guid.NewGuid(),
                NameFr = "Tech",
                NameEn = "Tech",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Role = UserRole.Organizer
        };

        _userRepositoryMock.Setup(repo => repo.GetOrganizer(organizerId)).Returns(organizer);

        // Act
        var result = _userService.GetUser(organizerId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(organizerId);
        result.Email.Should().Be(organizer.Email);

        _userRepositoryMock.Verify(repo => repo.GetOrganizer(organizerId), Times.Once);
    }

    [Fact]
    public void GetUser_ShouldReturnUserResponseDTO_WhenModeratorIsFoundById()
    {
        // Arrange
        var moderatorId = "Moderator";
        var moderator = new User
        {
            Id = moderatorId,
            Email = "jane.doe@example.com",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Role = UserRole.Moderator
        };

        _userRepositoryMock.Setup(repo => repo.GetOrganizer(moderatorId)).Returns((User?)null); // Simulate no organizer found
        _userRepositoryMock.Setup(repo => repo.GetModerator(moderatorId)).Returns(moderator); // Simulate moderator found

        // Act
        var result = _userService.GetUser(moderatorId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(moderatorId);
        result.Email.Should().Be(moderator.Email);

        _userRepositoryMock.Verify(repo => repo.GetOrganizer(moderatorId), Times.Once);
        _userRepositoryMock.Verify(repo => repo.GetModerator(moderatorId), Times.Once);
    }

    [Fact]
    public void GetUser_ShouldThrowException_WhenNoUserIsAssociatedWithProvidedId()
    {
        // Arrange
        var userId = "nobody";

        // Setup both organizer and moderator repositories to return null, simulating that no user is found with the provided ID
        _userRepositoryMock.Setup(repo => repo.GetOrganizer(userId)).Returns(null as User);
        _userRepositoryMock.Setup(repo => repo.GetModerator(userId)).Returns(null as User);

        // Act
        Action act = () => _userService.GetUser(userId);

        // Assert
        act.Should().Throw<Exception>().WithMessage("No users associated with this ID");
        _userRepositoryMock.Verify(repo => repo.GetOrganizer(userId), Times.Once);
        _userRepositoryMock.Verify(repo => repo.GetOrganizer(userId), Times.Once);
    }


    [Fact]
    public void UpdateUser_ShouldReturnTrue_WhenOrganizerIsUpdatedSuccessfully()
    {
        // Arrange
        var organizerId = "jane-doe";
        var actAreaIdModified = Guid.NewGuid();
        var updateDto = new UserUpdateDTO
        {
            Email = "jane.doe@example.com",
            Organization = "NewOrg",
            ActivityAreaId = actAreaIdModified,
            Id = organizerId
        };

        var activity = new ActivityArea
        {
            Id = actAreaIdModified,
            NameFr = "Tech",
        };

        var existingOrganizer = new User
        {
            Id = organizerId,
            Email = "john.doe@example.com",
            Organization = "ExampleOrg",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Role = UserRole.Organizer
        };

        _activityAreaRepositoryMock.Setup(repo => repo.Get(actAreaIdModified)).Returns(activity); // Simulate activity area found
        _userRepositoryMock.Setup(repo => repo.GetOrganizer(organizerId)).Returns(existingOrganizer);
        _userRepositoryMock.Setup(repo => repo.Update(organizerId, It.IsAny<User>())).Returns(true);

        // Act
        var result = _userService.UpdateUser(organizerId, updateDto);

        // Assert
        result.Should().BeTrue();

        _userRepositoryMock.Verify(repo => repo.Update(organizerId, It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public void UpdateUser_ShouldThrow_WhenActivityAreaIsNotFoundInTheList()
    {
        // Arrange
        var organizerId = "org";
        var badActAreaIdModified = Guid.NewGuid();
        var updateDto = new UserUpdateDTO
        {
            Email = "jane.doe@example.com",
            Organization = "NewOrg",
            ActivityAreaId = badActAreaIdModified,
            Id = organizerId
        };

        var existingOrganizer = new User
        {
            Id = organizerId,
            Email = "john.doe@example.com",
            Organization = "ExampleOrg",
            ActivityArea = new ActivityArea
            {
                Id = Guid.NewGuid(),
                NameFr = "Tech",
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _activityAreaRepositoryMock.Setup(repo => repo.Get(badActAreaIdModified)).Returns(null as ActivityArea); // Simulate activity area not found
        _userRepositoryMock.Setup(repo => repo.GetOrganizer(organizerId)).Returns(existingOrganizer);
        _userRepositoryMock.Setup(repo => repo.Update(organizerId, It.IsAny<User>())).Returns(true);

        // Act & Assert
        Assert.Throws<NotFoundException<ActivityArea>>(() => _userService.UpdateUser(organizerId, updateDto));
    }


    [Fact]
    public void UpdateUser_ShouldReturnTrue_WhenModeratorIsUpdatedSuccessfully()
    {
        // Arrange
        var moderatorId = "mod";
        var updateDto = new UserUpdateDTO
        {
            Email = "john.updated@example.com",
            Id = moderatorId
        };

        var existingModerator = new User
        {
            Id = moderatorId,
            Email = "john.doe@example.com",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Role = UserRole.Moderator
        };

        _userRepositoryMock.Setup(repo => repo.GetModerator(moderatorId)).Returns(existingModerator); // Simulate moderator found
        _userRepositoryMock.Setup(repo => repo.Update(moderatorId, It.IsAny<User>())).Returns(true);

        // Act
        var result = _userService.UpdateUser(moderatorId, updateDto);

        // Assert
        result.Should().BeTrue();

        _userRepositoryMock.Verify(repo => repo.Update(moderatorId, It.IsAny<User>()), Times.Once);
    }
}
