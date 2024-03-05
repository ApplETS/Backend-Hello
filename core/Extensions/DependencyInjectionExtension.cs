using api.core.Misc;
using api.core.repositories;
using api.core.repositories.abstractions;
using api.core.services.abstractions;
using api.core.Services;
using api.emails.Services;
using api.emails.Services.Abstractions;
using api.files.Services;
using api.files.Services.Abstractions;

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
        services.AddTransient<IModeratorRepository, ModeratorRepository>();

        // Services
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IEventService, EventService>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IFileShareService, FileShareService>();
        services.AddTransient<ITagService, TagService>();

        return services;
    }
}