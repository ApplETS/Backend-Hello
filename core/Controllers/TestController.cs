using api.core.Data.Requests;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;


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
        
        Dictionary<string, string> queryParameters = new();
        
        queryParameters["client_id"] = Environment.GetEnvironmentVariable("OPENID_CLIENT_ID");
        queryParameters["response_type"] = "code";
        queryParameters["redirect_uri"] = "http://localhost:8080";
        queryParameters["scope"] = "email";
        queryParameters["state"] = "1234";
        
        return Redirect(redirectionURL + string.Join('&', queryParameters.Select(qp => qp.Key + '=' + qp.Value)));
    }

    [HttpGet]
    [Route("/")]
    public async Task<IActionResult> Reception([FromQuery] string code, [FromQuery] string? state)
    {
        using HttpClient client = new(new HttpClientHandler
        {
            PreAuthenticate = true,
        })
;
        string claimUrl = Environment.GetEnvironmentVariable("OPENID_BASE_URL") + "token/";

        Dictionary<string, string> body = new()
        {
            ["grant_type"] = "authorization_code",
            ["redirect_uri"] = Request.Scheme + "://" + Request.Host.Value,
            ["code"] = code
        };

        HttpResponseMessage response = await client.PostAsync(claimUrl, new FormUrlEncodedContent(body));




        return Ok();
    }
}