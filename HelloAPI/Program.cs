using HelloAPI.Data;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var postgresqlConnectionString = Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION_STR") ?? throw new Exception("POSTGRESQL_CONNECTION_STR is not set");
builder.Services.AddDbContext<HelloContext>(o =>
{
    o.UseNpgsql(postgresqlConnectionString);
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
