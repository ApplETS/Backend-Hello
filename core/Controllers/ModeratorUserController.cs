using api.core.data.entities;
using api.core.Data;
using api.core.Data.requests;
using api.core.Data.Responses;
using api.core.Misc;
using api.core.services.abstractions;
using api.core.Services.Abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.core.controllers;

[Authorize]
[ApiController]
[Route("api/moderator/organizer")]
public class ModeratorUserController(IUserService userService, IAuthService authService) : ControllerBase
{
    [HttpPost]
    public IActionResult CreateOrganizer([FromBody] UserCreateDTO organizer)
    {
        EnsureIsModerator();

        var strongPassword = GenerateRandomPassword(12);
        var supabaseUser = authService.SignUp(organizer.Email, strongPassword);
        Guid.TryParse(supabaseUser, out Guid userId);
        var created = userService.AddOrganizer(userId, organizer);

        return Ok(created);
    }

    [HttpGet]
    public IActionResult GetUsers()
    {
        EnsureIsModerator();

        var users = userService.GetUsers();

        return Ok(new Response<IEnumerable<UserResponseDTO>> { Data = users });
    }

    private void EnsureIsModerator()
    {
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        var user = userService.GetUser(userId);
        if (user != null && user.Type != "Moderator")
            throw new UnauthorizedAccessException();
    }

    private string GenerateRandomPassword(int length)
    {
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!?.$_";
        Random random = new Random();
        char[] chars = new char[length];

        for (int i = 0; i < length; i++)
        {
            chars[i] = validChars[random.Next(validChars.Length)];
        }

        return new string(chars);
    }
}
