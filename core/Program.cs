using api.core.Extensions;

using System.Text;

using api.core.data;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using api.core.Misc;
using api.emails;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Environments setup
string supabaseSecretKey = null!;
string supabaseProjectId = null!;
string connectionString = null!;
string? redisConnString = null!;

connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? throw new Exception("CONNECTION_STRING is not set");

redisConnString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");

if (!EF.IsDesignTime)
{
    supabaseSecretKey = Environment.GetEnvironmentVariable("SUPABASE_SECRET_KEY") ?? throw new Exception("SUPABASE_SECRET_KEY is not set");
    supabaseProjectId = Environment.GetEnvironmentVariable("SUPABASE_PROJECT_ID") ?? throw new Exception("SUPABASE_PROJECT_ID is not set");
}

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDbContext<EventManagementContext>(opt => opt.UseNpgsql(connectionString));

builder.Services.AddAuthentication().AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(supabaseSecretKey)),
        ValidAudiences = ["authenticated"],
        ValidIssuer = $"https://{supabaseProjectId}.supabase.co/auth/v1"
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
    options.AddBasePolicy(builder =>
        builder.Cache());
});

// Errors handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Endpoints
builder.Services.AddControllers();

builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString);

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

app.UseAuthorization();

app.MapHealthChecks("/health");

await app.Services.AddSchedulerAsync();

if (redisConnString != null)
    app.UseOutputCache();

app.MapControllers();

app.Run();
