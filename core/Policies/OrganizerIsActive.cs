using api.core.Data.Exceptions;
using api.core.services.abstractions;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace api.core.Policies;

public class OrganizerIsActiveRequirement : IAuthorizationRequirement { }

public class OrganizerIsActiveHandler(IUserService userService) : AuthorizationHandler<OrganizerIsActiveRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OrganizerIsActiveRequirement requirement)
    {
        var identifierClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var userId = Guid.Parse(identifierClaim ?? throw new UnauthorizedException());

        var user = userService.GetUser(userId);
        if (user.Type != "Organizer" || !user.IsActive)
            throw new UnauthorizedException();
        else
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
