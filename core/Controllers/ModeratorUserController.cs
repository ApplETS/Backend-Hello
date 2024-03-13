﻿using api.core.data.entities;
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
        await emailService.SendEmailAsync<UserCreationModel>(
            organizer.Email,
            "Votre compte Hello!",
            new UserCreationModel
            {
                Salutation = $"Bonjour {organizer.Organisation},",
                AccountCreatedText = "Votre compte Hello a été créé!",
                TemporaryPasswordHeader = "Votre mot de passe temporaire est: ",
                TemporaryPassword = strongPassword,
                LoginButtonText = "Se connecter",
                // We will probably want this in a .env or settings file for the actual site later on
                ButtonLink = new Uri("http://localhost:3000/fr/login")
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