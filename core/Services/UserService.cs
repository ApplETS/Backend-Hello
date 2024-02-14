using api.core.data.entities;
using api.core.Data.Exceptions;
using api.core.Data.requests;
using api.core.Data.Responses;
using api.core.repositories.abstractions;
using api.core.services.abstractions;

namespace api.core.Services;

public class UserService(IOrganizerRepository organizerRepository, IModeratorRepository moderatorRepository) : IUserService
{
    public UserResponseDTO AddOrganizer(UserRequestDTO organizerDto)
    {
        var inserted = organizerRepository.Add(new Organizer
        {
            Id = organizerDto.Id,
            Email = organizerDto.Email,
            Organisation = organizerDto.Organisation ?? "",
            ActivityArea = organizerDto.ActivityArea ?? "",
            ProfileDescription = organizerDto.ProfileDescription ?? "",
            FacebookLink = organizerDto.FacebookLink,
            InstagramLink = organizerDto.InstagramLink,
            TikTokLink = organizerDto.TikTokLink,
            XLink = organizerDto.XLink,
            DiscordLink = organizerDto.DiscordLink,
            LinkedInLink = organizerDto.LinkedInLink,
            RedditLink = organizerDto.RedditLink,
            WebSiteLink = organizerDto.WebSiteLink,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        return UserResponseDTO.Map(inserted);
    }

    public UserResponseDTO GetUser(Guid id)
    {
        var organizer = organizerRepository.Get(id);
        if (organizer != null)
            return UserResponseDTO.Map(organizer!);

        var moderator = moderatorRepository.Get(id);
        if (moderator != null)
            return UserResponseDTO.Map(moderator!);

        throw new Exception("No users associated with this ID");
    }

    public bool UpdateUser(Guid id, UserRequestDTO dto)
    {
        var user = GetUser(id);

        switch (user.Type)
        {
            case "Moderator":
                return moderatorRepository.Update(id, new Moderator
                {
                    Id = id,
                    Email = dto.Email,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = DateTime.UtcNow
                });
            case "Organizer":
                return organizerRepository.Update(id, new Organizer
                {
                    Id = id,
                    Email = dto.Email,
                    Organisation = dto.Organisation,
                    ActivityArea = dto.ActivityArea,
                    ProfileDescription = dto.ProfileDescription,
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
                });
            default:
                throw new Exception("No users associated witht thid ID can be updated");
        }
    }
}
