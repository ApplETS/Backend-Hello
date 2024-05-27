using api.core.Data;
using api.core.Data.requests;
using api.core.Data.Responses;
using api.core.Misc;
using api.core.services.abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.core.controllers;

/// <summary>
/// A controller to handle calls to your own user. This allows calling function to yourself
/// without having to know your own id. This is useful for updating your own information or fetching 
/// your profile information. This can work for either a moderator or an organizer.
/// 
/// Under the hood, this controller uses the IUserService to manage the data by passing the userId found in 
/// your JWT token.
/// </summary>
/// <param name="userService">The User Service will allows managing your user's data</param>
[Authorize]
[ApiController]
[Route("api/me")]
public class MeController(IUserService userService) : ControllerBase
{
    /// <summary>
    /// Get the user connected to the API using the JWT token
    /// </summary>
    /// <returns>The user connected data</returns>
    [HttpGet]
    public IActionResult GetUser()
    {
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        var organizer = userService.GetUser(userId);

        return new OkObjectResult(
             new Response<UserResponseDTO>
             {
                 Data = organizer,
             });
    }

    /// <summary>
    /// Modify the user's connected data
    /// </summary>
    /// <param name="user">a user DTO object to allow modifying info </param>
    /// <returns>returns 200 OK if everything went well or 400 Bad request if there was an error with the data sent out</returns>
    [HttpPatch]
    public IActionResult UpdateUser([FromBody] UserUpdateDTO user)
    {
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        return userService.UpdateUser(userId, user) ? Ok() : BadRequest();
    }

    /// <summary>
    /// Update the user connected avatar
    /// </summary>
    /// <param name="avatarReq">The update form data to modify an avatar</param>
    /// <returns>The url of the downloadable avatar file</returns>
    [HttpPatch("avatar")]
    public IActionResult UpdateUserAvatar([FromForm] UserAvatarUpdateDTO avatarReq)
    {
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        var url = userService.UpdateUserAvatar(userId, avatarReq.avatarFile);

        return new OkObjectResult(
            new Response<string>
            {
                Data = url,
            });
    }
}
