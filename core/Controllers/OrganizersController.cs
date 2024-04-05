using api.core.data.entities;
using api.core.Data;
using api.core.Data.Enums;
using api.core.Data.Exceptions;
using api.core.Data.requests;
using api.core.Data.Requests;
using api.core.Data.Responses;
using api.core.Misc;
using api.core.services.abstractions;
using api.emails.Models;
using api.emails.Services.Abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.core.controllers;

[Authorize(Policy = AuthPolicies.IsModerator)]
[ApiController]
[Route("api/moderator/organizer")]
public class ModeratorUserController(IUserService userService, IAuthService authService, IEmailService emailService) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("{organizerId}")]
    public IActionResult GetOrganizer(Guid organizerId)
    {
        var user = userService.GetUser(organizerId);
        return user.Type == "Organizer" ?
            Ok(new Response<UserResponseDTO>
            {
                Data = user
            })
            : throw new NotFoundException<Organizer>();
    }

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
                Title = "Création de votre compte Hello!",
                Salutation = $"Bonjour {organizer.Organization},",
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
    public IActionResult GetUsers(string? search, OrganizerAccountActiveFilter filter, [FromQuery] PaginationRequest pagination)
    {
        var validFilter = new PaginationRequest(pagination.PageNumber, pagination.PageSize);
        var users = userService.GetUsers(search, filter, out int totalRecords);

        var paginatedRes = users
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList()
            .Select((e) =>
            {
                var url = userService.GetUserAvatarUrl(e.Id);
                e.AvatarUrl = url;
                return e;
            })
            .ToList();

        var response = PaginationHelper.CreatePaginatedReponse(paginatedRes, validFilter, totalRecords);
        return Ok(response);
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
                    Title = "Désactivation de votre compte Hello",
                    Salutation = $"Bonjour {organizer.Organization},",
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
