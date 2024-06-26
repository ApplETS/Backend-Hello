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

namespace api.core.controllers;


/// <summary>
/// A controller to handle calls to manage the organizers. This controller is only accessible to moderators.
/// A moderator can create, get, toggle the active state of an organizer and get a list of all organizers.
/// 
/// Under the hood, this controller uses the IUserService to manage the data. It also uses the IAuthService to
/// create a new user in the Supabase database. Finally, it uses the IEmailService to send an email to the newly
/// created organizer with a temporary password. All the EMAIL_ env must be configured in the environment variables.
/// for this to work.
/// </summary>
/// <param name="userService">Used to fetch and manage the organizers</param>
/// <param name="authService">Used to create a new user in the Supabase database</param>
/// <param name="emailService">Used to send an email to the newly created organizer</param>
/// <param name="configuration">Used to fetch the FRONTEND_BASE_URL from the environments variables</param>
[Authorize(Policy = AuthPolicies.IsModerator)]
[ApiController]
[Route("api/organizers")]
public class ModeratorUserController(
    IUserService userService,
    IAuthService authService,
    IEmailService emailService,
    IConfiguration configuration) : ControllerBase
{

    /// <summary>
    /// Get a specific organizer by its id.
    /// 
    /// This function is specifically bypassing the authorization policy to allow anyone to get an organizer.
    /// </summary>
    /// <param name="organizerId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException{Organizer}"></exception>
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

    /// <summary>
    /// Create an organizer
    /// 
    /// TODO: would be nice to move the organizer creation in a separate service
    /// </summary>
    /// <param name="organizer"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost]
    public async Task<IActionResult> CreateOrganizer([FromBody] UserCreateDTO organizer)
    {
        var strongPassword = GenerateRandomPassword(12);
        var supabaseUser = authService.SignUp(organizer.Email, strongPassword);
        _ = Guid.TryParse(supabaseUser, out Guid userId);
        var created = userService.AddOrganizer(userId, organizer);
        var frontBaseUrl = configuration.GetValue<string>("FRONTEND_BASE_URL") ?? throw new ArgumentNullException("FRONTEND_BASE_URL is not set");
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

    /// <summary>
    /// Get all users with pagination and search term
    /// </summary>
    /// <param name="search">an optional search field to filter data</param>
    /// <param name="filter">filter based on different attributes define in OrganizerAccountActiveFilter</param>
    /// <param name="pagination">paginate the data using the correct attributes</param>
    /// <returns></returns>
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

    /// <summary>
    /// Toggle an organizer active state
    /// </summary>
    /// <param name="organizerId">the organizer tyou want to enable</param>
    /// <param name="reason">pass a reason for the toggle active change, will be send by email</param>
    /// <returns></returns>
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

    /// <summary>
    /// Generate a random password of a given length
    /// </summary>
    /// <param name="length">the given length of the random password</param>
    /// <returns>A random password</returns>
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
