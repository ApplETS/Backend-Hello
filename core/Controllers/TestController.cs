using api.core.Data.Requests;

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
        
        Dictionary<string, string> queryParameters = new();
        
        queryParameters["client_id"] = Environment.GetEnvironmentVariable("OPENID_CLIENT_ID");
        queryParameters["response_type"] = "code";
        queryParameters["redirect_uri"] = "http://localhost:8080";
        queryParameters["scope"] = "email";
        //queryParameters["response_type"] = "code";
        
        return Redirect(redirectionURL + string.Join('&', queryParameters.Select(qp => qp.Key + '=' + qp.Value)));
    }

}