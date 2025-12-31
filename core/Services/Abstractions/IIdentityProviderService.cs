namespace api.core.Services.Abstractions;

/// <summary>
/// Interface to communicate with any ID Provider
/// </summary>
public interface IIdentityProviderService
{
    /// <summary>
    /// Fetches User's information from the ID Provider
    /// </summary>
    /// <param name="accessHeader">Header used to call the endpoint</param>
    /// <returns>If result is null, UserInfo endpoint cannot be accessed</returns>
    UserInfoDto? GetUserInfo(string accessHeader);
}

public class UserInfoDto
{
    public string Sub { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool EmailVerified { get; set; }
    public string Name { get; set; } = null!;
    public string GivenName { get; set; } = null!;
    public string PreferedUsername { get; set; } = null!;
    public string Nickname { get; set; } = null!;
    public List<string> Groups { get; set; } = null!;
}