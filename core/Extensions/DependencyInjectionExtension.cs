using api.core.Misc;
using api.core.repositories;
using api.core.repositories.abstractions;
using api.core.Repositories.Abstractions;
using api.core.services.abstractions;
using api.core.Services;

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

        return services;
    }
}