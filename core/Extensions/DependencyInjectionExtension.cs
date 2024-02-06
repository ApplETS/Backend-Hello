using api.core.Misc;
using api.core.repositories;
using api.core.repositories.abstractions;
using api.core.Repositories.Abstractions;
using api.core.services.abstractions;
using api.core.Services;
using api.emails.Services;
using api.emails.Services.Abstractions;

namespace api.core.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        // Middlewares
        services.AddTransient<CustomExceptionsCheckerMiddleware>();

        // Repositories
        services.AddTransient<IOrganizerRepository, OrganizerRepository>();
        services.AddTransient<ITagRepository, TagRepository>();
        services.AddTransient<IEventRepository, EventRepository>();

        // Services
        services.AddTransient<IOrganizerService, OrganizerService>();
        services.AddTransient<IEventService, EventService>();
        services.AddTransient<IEmailService, EmailService>();

        return services;
    }
}