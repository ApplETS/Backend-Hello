using api.core.data.entities;
using api.core.Data;
using api.core.Data.Exceptions;
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

[Authorize]
[ApiController]
[Route("api/moderator/organizer")]
public class ModeratorUserController(IUserService userService, IAuthService authService, IEmailService emailService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrganizer([FromBody] UserCreateDTO organizer)
    {
        EnsureIsModerator();

        var strongPassword = GenerateRandomPassword(12);
        var supabaseUser = authService.SignUp(organizer.Email, strongPassword);
        Guid.TryParse(supabaseUser, out Guid userId);
        var created = userService.AddOrganizer(userId, organizer);
        var frontBaseUrl = Environment.GetEnvironmentVariable("FRONTEND_BASE_URL") ?? throw new Exception("FRONTEND_BASE_URL is not set");
        await emailService.SendEmailAsync<UserCreationModel>(
            organizer.Email,
            "Votre compte Hello!",
            new UserCreationModel
            {
                Title = "Création de compte Hello!",
                Salutation = $"Bonjour {organizer.Organization},",
                AccountCreatedText = "Votre compte Hello a été créé!",
                TemporaryPasswordHeader = "Voici votre mot de passe temporaire, assurez-vous de suivre les indications pour le modifier lors de votre première connexion. ",
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
        EnsureIsModerator();

        var users = userService.GetUsers();

        return Ok(new Response<IEnumerable<UserResponseDTO>> { Data = users });
    }

    private void EnsureIsModerator()
    {
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        var user = userService.GetUser(userId);
        if (user != null && user.Type != "Moderator")
            throw new UnauthorizedException();
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
