using api.core.Services.Abstractions;

using System.Text.Json;

namespace api.core.Services;

/// <summary>
/// Service used to communicate with Authentik ID Provider
/// </summary>
public class AuthentikService : IIdentityProviderService
{
    public UserInfoDto? GetUserInfo(string accessHeader)
    {
        using HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri(Environment.GetEnvironmentVariable("OPENID_BASE_URL"))
        };

        using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "userinfo");
        request.Headers.Add("Authorization", accessHeader);

        // TODO : Changer pour un comportement asynchrone
        var response = client.SendAsync(request).Result;

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return response.Content.ReadFromJsonAsync<UserInfoDto>(new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower }).Result;
    }
}
