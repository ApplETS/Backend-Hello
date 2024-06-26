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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO req, CancellationToken ct)
    {
        var projectId = configuration.GetValue<string>("SUPABASE_PROJECT_ID");
        var anonKey = configuration.GetValue<string>("SUPABASE_ANON_KEY");

        var client = new Supabase.Client($"https://{projectId}.supabase.co", anonKey);
        var response = await client.Auth.SignInWithPassword(req.Email, req.Password);
        return Ok(response);
    }
}