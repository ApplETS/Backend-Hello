using Microsoft.AspNetCore.Authorization;
using api.core.Policies;
using api.core.Misc;

namespace api.core.Extensions;

public static class PoliciesExtension
{
    public static IServiceCollection AddPolicies(this IServiceCollection services)
    {
        services.AddTransient<IAuthorizationHandler, IsModeratorHandler>();
        services.AddTransient<IAuthorizationHandler, OrganizerIsActiveHandler>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthPolicies.IsModerator, policy => policy.Requirements.Add(new IsModeratorRequirement()));
            options.AddPolicy(AuthPolicies.OrganizerIsActive, policy => policy.Requirements.Add(new OrganizerIsActiveRequirement()));
        });

        return services;
    }
}
