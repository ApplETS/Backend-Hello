using api.core.data.entities;
using api.core.Data.Exceptions;
using api.core.Data.Enums;
using api.core.Data.requests;
using api.core.Data.Responses;
using api.core.repositories;
using api.core.repositories.abstractions;
using api.core.services.abstractions;
using api.files.Services.Abstractions;

using Microsoft.IdentityModel.Tokens;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace api.core.Services;

public class UserService(
    IOrganizerRepository organizerRepository,
    IFileShareService fileShareService,
    IModeratorRepository moderatorRepository,
    ITagRepository tagRepository,
    IActivityAreaRepository activityAreaRepository) : IUserService
{
    private const double IMAGE_RATIO_SIZE_ACCEPTANCE = 1.0; // width/height ratio avatar
    private const double TOLERANCE_ACCEPTABILITY = 0.001;
    private const string AVATAR_FILE_NAME = "avatar.webp";

    public UserResponseDTO AddOrganizer(Guid id, UserCreateDTO organizerDto)
    {
        if (organizerDto.ActivityAreaId != null)
        {
            var activityArea = activityAreaRepository.Get(organizerDto.ActivityAreaId.Value);
            NotFoundException<ActivityArea>.ThrowIfNull(activityArea);
        }

        var inserted = organizerRepository.Add(new Organizer
        {
            Id = id,
            Email = organizerDto.Email,
            Organization = organizerDto.Organization ?? "",
            ActivityAreaId = organizerDto.ActivityAreaId,
            ProfileDescription = "",
            IsActive = true,
            HasLoggedIn = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        var avatarUri = fileShareService.FileGetDownloadUri($"{id}/{AVATAR_FILE_NAME}");
        var user = UserResponseDTO.Map(inserted);
        user.AvatarUrl = avatarUri.ToString();
        
        return user;
    }

    public UserResponseDTO GetUser(Guid id)
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

    public string GetUserAvatarUrl(Guid id)
    {
        var avatarUri = fileShareService.FileGetDownloadUri($"{id}/{AVATAR_FILE_NAME}");
        return avatarUri.ToString();
    }

    public IEnumerable<UserResponseDTO> GetUsers(string? search, OrganizerAccountActiveFilter activeFilter, out int count)
    {
        var organizers = organizerRepository.GetAll()
            .Where(x => (search.IsNullOrEmpty() ||
                x.Organization.ToLower().Contains(search!.ToLower() ?? "") ||
                x.Email.ToLower().Contains(search!.ToLower() ?? "")) &&
                ((activeFilter.HasFlag(OrganizerAccountActiveFilter.Active) && x.IsActive) ||
                 (activeFilter.HasFlag(OrganizerAccountActiveFilter.Inactive) && !x.IsActive) ||
                 activeFilter.HasFlag(OrganizerAccountActiveFilter.All))
            );
        count = organizers.Count();

        return organizers.Select(UserResponseDTO.Map);
    }

    public bool ToggleUserActiveState(Guid id)
    {
        EnsureIsOrganizer(id);

        var user = organizerRepository.Get(id);
        user!.IsActive = !user.IsActive;
        return organizerRepository.Update(id, user);
    }

    private void EnsureIsOrganizer(Guid id)
    {
        var user = GetUser(id);

        if (user.Type == "Moderator")
            throw new Exception("Moderators cannot be disabled");
    }

    public bool UpdateUser(Guid id, UserUpdateDTO dto)
    {
        var user = GetUser(id);

        if (dto.ActivityAreaId != null)
        {
            var activityArea = activityAreaRepository.Get(dto.ActivityAreaId.Value);
            NotFoundException<ActivityArea>.ThrowIfNull(activityArea);
        }

        return user.Type switch
        {
            "Moderator" => moderatorRepository.Update(id, new Moderator
            {
                Id = id,
                Email = dto.Email,
                CreatedAt = user.CreatedAt,
                UpdatedAt = DateTime.UtcNow
            }),
            "Organizer" => organizerRepository.Update(id, new Organizer
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

    public string UpdateUserAvatar(Guid id, IFormFile avatarFile)
    {
        _ = GetUser(id);
        var userId = id.ToString();
        HandleImageSaving(userId, avatarFile);
        var url = fileShareService.FileGetDownloadUri($"{userId}/{AVATAR_FILE_NAME}");
        return url.ToString();
    }

    private void HandleImageSaving(string directory, IFormFile imageFile)
    {
        byte[] imageBytes = [];
        try
        {
            using var image = Image.Load(imageFile.OpenReadStream());
            int width = image.Size.Width;
            int height = image.Size.Height;

            if (Math.Abs((width / height) - IMAGE_RATIO_SIZE_ACCEPTANCE) > TOLERANCE_ACCEPTABILITY)
                throw new BadParameterException<Event>(nameof(image), "Invalid image aspect ratio");

            image.Mutate(c => c.Resize(200, 200));
            using var outputStream = new MemoryStream();
            image.SaveAsWebp(outputStream);
            outputStream.Position = 0;

            fileShareService.FileUpload(directory, AVATAR_FILE_NAME, outputStream);
        }
        catch (Exception e)
        {
            throw new BadParameterException<Event>(nameof(imageFile), $"Invalid image metadata: {e.Message}");
        }
    }
}
