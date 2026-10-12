using Microsoft.EntityFrameworkCore;
using PlusOne.Api.Data;
using PlusOne.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var databaseConnection = builder.Configuration.GetConnectionString("PlusOneDatabase")
    ?? throw new InvalidOperationException("Connection string 'PlusOneDatabase' is not configured.");
builder.Services.AddDbContext<PlusOneDbContext>(options =>
    options.UseSqlite(databaseConnection));
builder.Services.AddScoped<IOfferRepository, SqliteOfferRepository>();
builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration
        .GetSection("Cors:AllowedOrigins")
        .Get<string[]>() ?? [];

    options.AddPolicy("Frontend", policy =>
    {
        if (allowedOrigins.Length > 0)
        {
            policy.WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    });
});

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var database = scope.ServiceProvider.GetRequiredService<PlusOneDbContext>();
    await database.Database.EnsureCreatedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseCors("Frontend");

app.MapGet("/health", () => Results.Ok(new HealthStatus("Healthy")))
    .WithName("GetHealth");

app.MapControllers();

app.Run();

record HealthStatus(string Status);

public partial class Program { }
