using api.core.data.entities;
using api.core.Data.Exceptions;
using api.core.Data.Enums;
using api.core.Data.requests;
using api.core.Data.Responses;
using api.core.repositories.abstractions;
using api.core.services.abstractions;
using api.files.Services.Abstractions;


using SixLabors.ImageSharp;
using api.core.Misc;
using api.core.Repositories.Abstractions;
using api.core.Data.Entities;

namespace api.core.Services;

public class UserService(
    IUserRepository userRepository,
    IFileShareService fileShareService,
    ITagRepository tagRepository,
    IActivityAreaRepository activityAreaRepository,
    IImageService imageService) : IUserService
{
    private const string AVATAR_FILE_NAME = "avatar.webp";

    public UserResponseDTO AddOrganizer(string id, UserCreateDTO organizerDto)
    {
        if (organizerDto.ActivityAreaId != null)
        {
            var activityArea = activityAreaRepository.Get(organizerDto.ActivityAreaId.Value);
            NotFoundException<ActivityArea>.ThrowIfNull(activityArea);
        }

        var inserted = userRepository.Add(new User
        {
            Id = id,
            Email = organizerDto.Email,
            Organization = organizerDto.Organization ?? "",
            ActivityAreaId = organizerDto.ActivityAreaId,
            ProfileDescription = "",
            IsActive = true,
            HasLoggedIn = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Role = UserRole.Organizer
        });

        var avatarUri = fileShareService.FileGetDownloadUri($"{id}/{AVATAR_FILE_NAME}");
        var user = UserResponseDTO.Map(inserted);
        user.AvatarUrl = avatarUri.ToString();

        return user;
    }

    public UserResponseDTO GetUser(string id)
    {
        UserResponseDTO? userRes = null;
        var organizer = organizerRepository.Get(id);
        if (organizer != null)
            userRes = UserResponseDTO.Map(organizer!);

        var moderator = moderatorRepository.Get(id);
        if (moderator != null)
            userRes = UserResponseDTO.Map(moderator!);

        if (userRes == null) throw new Exception("No users associated with this ID");

        var fields = tagRepository.GetInterestFieldsForOrganizer(id);
        userRes.FieldsOfInterests = fields;

        var avatarUri = fileShareService.FileGetDownloadUri($"{id}/{AVATAR_FILE_NAME}");
        userRes.AvatarUrl = avatarUri.ToString();

        return userRes;
    }

    public string GetUserAvatarUrl(string id)
    {
        var avatarUri = fileShareService.FileGetDownloadUri($"{id}/{AVATAR_FILE_NAME}");
        return avatarUri.ToString();
    }

    public IEnumerable<UserResponseDTO> GetUsers(string? search, OrganizerAccountActiveFilter activeFilter, out int count)
    {
        var organizers = userRepository.GetAll()
            .Where(x => (search == null || search.Equals("") ||
                x.Organization.ToLower().Contains(search!.ToLower() ?? "") ||
                x.Email.ToLower().Contains(search!.ToLower() ?? "")) &&
                ((activeFilter.HasFlag(OrganizerAccountActiveFilter.Active) && x.IsActive) ||
                 (activeFilter.HasFlag(OrganizerAccountActiveFilter.Inactive) && !x.IsActive) ||
                 activeFilter.HasFlag(OrganizerAccountActiveFilter.All))
            );
        count = organizers.Count();

        return organizers.Select(UserResponseDTO.Map);
    }

    public bool ToggleUserActiveState(string id)
    {
        EnsureIsOrganizer(id);

        var user = userRepository.Get(id);
        user!.IsActive = !user.IsActive;
        return userRepository.Update(id, user);
    }

    private void EnsureIsOrganizer(string id)
    {
        var user = GetUser(id);

        if (user.Type == "Moderator")
            throw new Exception("Moderators cannot be disabled");
    }

    public bool UpdateUser(string id, UserUpdateDTO dto)
    {
        var user = GetUser(id);

        if (dto.ActivityAreaId != null)
        {
            var activityArea = activityAreaRepository.Get(dto.ActivityAreaId.Value);
            NotFoundException<ActivityArea>.ThrowIfNull(activityArea);
        }

        return user.Type switch
        {
            "Moderator" => userRepository.Update(id, new User
            {
                Id = id,
                Email = dto.Email,
                CreatedAt = user.CreatedAt,
                UpdatedAt = DateTime.UtcNow
            }),
            "Organizer" => userRepository.Update(id, new User
            {
                Id = id,
                Email = dto.Email,
                Organization = dto.Organization ?? "",
                ActivityAreaId = dto.ActivityAreaId,
                ProfileDescription = dto.ProfileDescription ?? "",
                IsActive = user.IsActive,
                HasLoggedIn = dto.HasLoggedIn ?? true,
                FacebookLink = dto.FacebookLink,
                InstagramLink = dto.InstagramLink,
                TikTokLink = dto.TikTokLink,
                XLink = dto.XLink,
                DiscordLink = dto.DiscordLink,
                LinkedInLink = dto.LinkedInLink,
                RedditLink = dto.RedditLink,
                WebSiteLink = dto.WebSiteLink,
                CreatedAt = user.CreatedAt,
                UpdatedAt = DateTime.UtcNow
            }),
            _ => throw new Exception("No users associated witht thid ID can be updated"),
        };
    }

    public string UpdateUserAvatar(string id, IFormFile avatarFile)
    {
        _ = GetUser(id);
        var userId = id.ToString();
        imageService.EnsureImageSizeAndStore(userId, avatarFile, ImageType.Avatar, AVATAR_FILE_NAME);
        var url = fileShareService.FileGetDownloadUri($"{userId}/{AVATAR_FILE_NAME}");
        return url.ToString();
    }
}
