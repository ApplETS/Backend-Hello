using System.Reflection;
using System.Text;

using api.core.data;
using api.core.Extensions;
using api.core.Misc;
using api.emails;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
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


var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("OPENID_CLIENT_SECRET") ?? "");
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    //.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    //{
    //    options.RequireHttpsMetadata = false;
    //    options.SaveToken = true;
    //    options.Authority = Environment.GetEnvironmentVariable("OPENID_ISSUER");

    //    options.TokenValidationParameters = new TokenValidationParameters
    //    {
    //        ValidateIssuer = true,
    //        ValidateAudience = true,
    //        ValidIssuer = Environment.GetEnvironmentVariable("OPENID_ISSUER"),
    //        ValidAudience = Environment.GetEnvironmentVariable("OPENID_CLIENT_ID"),
    //        NameClaimType = "email"
    //    };
    //})
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        // The URL of your Identity Provider (e.g., "https://dev-xyz.us.auth0.com/")
        // The API will download the public keys from here automatically.
        options.Authority = Environment.GetEnvironmentVariable("OPENID_ISSUER");

        // Who is this token for? (This usually matches the "API Identifier" in your IdP)
        options.Audience = Environment.GetEnvironmentVariable("OPENID_CLIENT_ID");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = Environment.GetEnvironmentVariable("OPENID_CLIENT_ID")
        };
        // Ensure HTTPS is used (should be true in production)
        options.RequireHttpsMetadata = true;
    });
//.AddOpenIdConnect(options =>
//   {
//       options.Authority = Environment.GetEnvironmentVariable("OPENID_ISSUER");
//       options.ClientId = Environment.GetEnvironmentVariable("OPENID_CLIENT_ID");
//       //options.ClientSecret = Environment.GetEnvironmentVariable("OPENID_CLIENT_SECRET");

//       //options.SaveTokens = false;

//       //// TODO : Mettre les scopes requis

//       options.Scope.Add("openid");
//       options.Scope.Add("email");
//       options.Scope.Add("profile");

//       //options.GetClaimsFromUserInfoEndpoint = true;

//       //options.TokenValidationParameters = new TokenValidationParameters
//       //{
//       //    NameClaimType = "email"
//       //};

//       //options.Events = new OpenIdConnectEvents
//       //{
//       //    OnTokenValidated = context =>
//       //    {
//       //        return Task.CompletedTask;
//       //    },
//       //    OnAuthenticationFailed = context =>
//       //    {
//       //        return Task.CompletedTask;
//       //    }
//       //};
//   });

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
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. " +
        "\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.",
        OpenIdConnectUrl = new Uri(Environment.GetEnvironmentVariable("OPENID_ISSUER")!)
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
