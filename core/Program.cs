using System.Reflection;
using System.Text;

using api.core.data;
using api.core.Extensions;
using api.core.Misc;
using api.emails;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


IdentityModelEventSource.ShowPII = true;


var builder = WebApplication.CreateBuilder(args);

// Environments setup
string connectionString = null!;
string? redisConnString = null!;

connectionString =
    Environment.GetEnvironmentVariable("CONNECTION_STRING")
    ?? throw new Exception("CONNECTION_STRING is not set");

redisConnString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDbContext<EventManagementContext>(opt => opt.UseNpgsql(connectionString));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           options.RequireHttpsMetadata = false;
           options.SaveToken = true;
           options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = "https://login.microsoftonline.com/188c27a3-86bf-4988-9c94-025a75fcf0d1/v2.0",
               ValidAudience = "bf42ef76-b599-4ab1-a015-6e4b8afa347b",
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(""))
           };
       });

builder.Services.SetupScheduler();

if (string.IsNullOrEmpty(redisConnString))
{
    builder.Services.AddStackExchangeRedisOutputCache(options =>
    {
        options.Configuration = redisConnString;
    });
}

builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder => builder.Cache());
});

// Errors handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Endpoints
builder.Services.AddControllers();

builder.Services.AddHealthChecks().AddNpgSql(connectionString);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. " +
       "\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
              new OpenApiSecurityScheme
              {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
              },
        Array.Empty<string>()
        }
    });
    options.UseInlineDefinitionsForEnums();

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddEmailService(builder.Configuration);

builder.Services.AddDependencyInjection();
builder.Services.AddPolicies();

builder.Services.AddRateLimiters();

var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();
await using var db = scope.ServiceProvider.GetService<EventManagementContext>();
await db!.Database.MigrateAsync();

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionMiddleware();

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");

await app.Services.AddSchedulerAsync();

if (redisConnString != null)
    app.UseOutputCache();

app.MapControllers();

app.Run();
