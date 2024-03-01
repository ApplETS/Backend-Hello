using api.core.Data.Requests;
using api.emails.Models;
using api.emails.Services.Abstractions;

using Microsoft.AspNetCore.Mvc;


namespace api.core.controllers;

[ApiController]
[Route("api/test")]
public class TestController(IEmailService service, IConfiguration configuration) : ControllerBase
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