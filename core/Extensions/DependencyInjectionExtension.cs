﻿using api.core.Misc;
using api.core.repositories;
using api.core.repositories.abstractions;
using api.core.services.abstractions;
using api.core.Services;
using api.core.Services.Abstractions;
using api.emails.Services;
using api.emails.Services.Abstractions;

using Pipelines.Sockets.Unofficial.Arenas;

using Supabase;

namespace api.core.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        AddSupabase(services);

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
        services.AddTransient<IAuthService, AuthService>();

        return services;
    }

    private static void AddSupabase(IServiceCollection services)
    {
        var url = $"https://{Environment.GetEnvironmentVariable("SUPABASE_PROJECT_ID")}.supabase.co";
        var key = Environment.GetEnvironmentVariable("SUPABASE_ANON_KEY");
        services.AddSingleton(provider => new Client(url, key));
    }
}