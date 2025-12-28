using api.core.Misc;
using api.core.repositories;
using api.core.repositories.abstractions;
using api.core.Repositories.Abstractions;
using api.core.services.abstractions;
using api.core.Services;
using api.core.Services.Abstractions;
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
        services.AddTransient<ITagRepository, TagRepository>();
        services.AddTransient<IEventRepository, EventRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IReportRepository, ReportRepository>();
        services.AddTransient<IActivityAreaRepository, ActivityAreaRepository>();
        services.AddTransient<ISubscriptionRepository, SubscriptionRepository>();
        services.AddTransient<INotificationRepository, NotificationRepository>();

        // Services
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IEventService, EventService>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IFileShareService, FileShareService>();
        services.AddTransient<ITagService, TagService>();
        services.AddTransient<IReportService, ReportService>();
        services.AddTransient<IActivityAreaService, ActivityAreaService>();
        services.AddTransient<IModeratorService, ModeratorService>();
        services.AddTransient<IDraftEventService, DraftEventService>();
        services.AddTransient<IImageService, ImageService>();
        services.AddTransient<ISubscriptionService, SubscriptionService>();
        services.AddTransient<INotificationService, NotificationService>();

        // Utils
        services.AddTransient<IJwtUtils, JwtUtils>();

        return services;
    }

}