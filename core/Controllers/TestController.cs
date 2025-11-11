using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Mvc;


namespace api.core.controllers;
/// <summary>
/// This is a controller mainly available for troubleshooting and testing purposes.
/// Or to allow quicker development of some features.
/// </summary>
/// <param name="configuration">Used to fetch the SUPABASE_PROJECT_ID and SUPABASE_ANON_KEY from the environment variables</param>
[ApiController]
[Route("api/test")]
public class TestController(IConfiguration configuration) : ControllerBase
{

    [HttpGet]
    public IActionResult Login()
    {
        var redirectionURL = Environment.GetEnvironmentVariable("OPENID_BASE_URL") + "authorize/?";
        
        Dictionary<string, string> queryParameters = new()
        {
            ["client_id"] = Environment.GetEnvironmentVariable("OPENID_CLIENT_ID"),
            ["response_type"] = "code",
            ["redirect_uri"] = "http://localhost:8080",
            ["scope"] = "email",
            //["state"] = "1234"
        };
        
        return Redirect(redirectionURL + string.Join('&', queryParameters.Select(qp => qp.Key + '=' + qp.Value)));
    }

    [HttpGet]
    [Route("/")]
    public async Task<IActionResult> Reception([FromQuery] string code, [FromQuery] string? state)
    {
        using HttpClient client = new();
        string claimUrl = Environment.GetEnvironmentVariable("OPENID_BASE_URL") + "token/";

        Dictionary<string, string> body = new()
        {
            ["grant_type"] = "authorization_code",
            ["redirect_uri"] = Request.Scheme + "://" + Request.Host.Value,
            ["code"] = code
        };

        string clientId = Environment.GetEnvironmentVariable("OPENID_CLIENT_ID");
        string clientSecret = Environment.GetEnvironmentVariable("OPENID_CLIENT_SECRET");

        using HttpRequestMessage request = new(HttpMethod.Post, claimUrl);
         
        request.Content = new FormUrlEncodedContent(body);
        request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")));

        HttpResponseMessage response = await client.SendAsync(request);

        if (! response.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        string contenu = await response.Content.ReadAsStringAsync();

        JsonSerializerOptions settings = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        TokenResponse token = JsonSerializer.Deserialize<TokenResponse>(contenu, settings)!;

        return Ok(token);
    }
}

[JsonSerializable(typeof(TokenResponse))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
public record class TokenResponse
{
    public string AccessToken { get; set; }
    public string TokenType { get; set; }
    public string Scope { get; set; }
    public string IdToken { get; set; }
    public int ExpiresIn { get; set; }
}