using api.core.Extensions;

using System.Text;

using api.core.data;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using api.core.Misc;

var builder = WebApplication.CreateBuilder(args);

var supabaseSecretKey = Environment.GetEnvironmentVariable("SUPABASE_SECRET_KEY") ?? throw new Exception("SUPABASE_SECRET_KEY is not set");
var supabaseProjectId = Environment.GetEnvironmentVariable("SUPABASE_PROJECT_ID") ?? throw new Exception("SUPABASE_PROJECT_ID is not set");
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? throw new Exception("CONNECTION_STRING is not set");

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
         new string[] {}
         }
     });
});
builder.Services.AddDependencyInjection();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<CustomExceptionsCheckerMiddleware>();

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHealthChecks("/health");

app.MapControllers();

app.Run();
