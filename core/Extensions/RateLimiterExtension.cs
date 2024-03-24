using System.Threading.RateLimiting;

using api.core.controllers;

using Microsoft.AspNetCore.RateLimiting;

namespace api.core.Extensions;

public static class RateLimiterExtension
{
    public static IServiceCollection AddRateLimiters(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            var windowInSeconds = int.Parse(Environment.GetEnvironmentVariable("RATE_LIMIT_TIME_WINDOW_SECONDS") ?? "10");
            var permitLimit = int.Parse(Environment.GetEnvironmentVariable("RATE_LIMIT_MAX_REQUESTS") ?? "4");

            var window = TimeSpan.FromSeconds(windowInSeconds);

            options.AddFixedWindowLimiter(EventsController.RATE_LIMITING_POLICY_NAME, limiterOptions =>
            {
                limiterOptions.PermitLimit = permitLimit;
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 0;
                limiterOptions.Window = window;
            });
        });

        return services;
    }
}
