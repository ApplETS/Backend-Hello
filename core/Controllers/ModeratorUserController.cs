using api.core.Data;
using api.core.Data.requests;
using api.core.Data.Responses;
using api.core.Misc;
using api.core.services.abstractions;
using api.core.Services.Abstractions;
using api.emails.Models;
using api.emails.Services.Abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.core.controllers;

[Authorize(Policy = AuthPolicies.IsModerator)]
[ApiController]
[Route("api/moderator/organizer")]
public class ModeratorUserController(IUserService userService, IAuthService authService, IEmailService emailService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrganizer([FromBody] UserCreateDTO organizer)
    {
        var strongPassword = GenerateRandomPassword(12);
        var supabaseUser = authService.SignUp(organizer.Email, strongPassword);
        Guid.TryParse(supabaseUser, out Guid userId);
        var created = userService.AddOrganizer(userId, organizer);
        var frontBaseUrl = Environment.GetEnvironmentVariable("FRONTEND_BASE_URL") ?? throw new Exception("FRONTEND_BASE_URL is not set");
        await emailService.SendEmailAsync(
            organizer.Email,
            "Votre compte Hello!",
            new UserCreationModel
            {
                Salutation = $"Bonjour {organizer.Organisation},",
                AccountCreatedText = "Votre compte Hello a été créé!",
                TemporaryPasswordHeader = "Votre mot de passe temporaire est: ",
                TemporaryPassword = strongPassword,
                LoginButtonText = "Se connecter",
                ButtonLink = new Uri($"{frontBaseUrl}/fr/login")
            },
            emails.EmailsUtils.UserCreationTemplate
        );

        return Ok(new Response<UserResponseDTO> { Data = created });
    }

    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = userService.GetUsers();
        return Ok(new Response<IEnumerable<UserResponseDTO>> { Data = users });
    }

    [HttpPatch("{organizerId}/toggle")]
    public async Task<IActionResult> ToggleOrganizer(Guid organizerId, [FromQuery] string? reason)
    {
        var success = userService.ToggleUserActiveState(organizerId);
        var organizer = userService.GetUser(organizerId);

        if (success && !organizer.IsActive)
        {
            await emailService.SendEmailAsync(
                organizer.Email,
                "Votre compte Hello a été désactivé",
                new UserDeactivationModel
                {
                    Salutation = $"Bonjour {organizer.Organisation},",
                    UserDeactivationHeader = "Votre compte Hello a été désactivé pour la raison suivate: ",
                    UserDeactivationReason = reason ?? "",
                },
                emails.EmailsUtils.UserDeactivationTemplate
            );
        }

        return success ? Ok() : BadRequest();
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
