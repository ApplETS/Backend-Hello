﻿namespace api.core.Data.requests;

public class UserCreateDTO
{
    public required string Email { get; set; }

    public string? Organisation { get; set; } = null!;

    public string? ActivityArea { get; set; } = null!;
}

public class UserUpdateDTO : UserCreateDTO
{
    public Guid Id { get; set; }

    public string? ProfileDescription { get; set; } = null!;

    public string? FacebookLink { get; set; }

    public string? InstagramLink { get; set; }

    public string? TikTokLink { get; set; }

    public string? XLink { get; set; }

    public string? DiscordLink { get; set; }

    public string? LinkedInLink { get; set; }

    public string? RedditLink { get; set; }

    public string? WebSiteLink { get; set; }
}
