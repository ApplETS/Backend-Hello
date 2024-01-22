using System.Text;

using api.core.data;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
