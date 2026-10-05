using PlusOne.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IOfferRepository, InMemoryOfferRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/health", () => Results.Ok(new HealthStatus("Healthy")))
    .WithName("GetHealth");

app.MapGet("/api/offers", async (IOfferRepository repository, CancellationToken cancellationToken) =>
        Results.Ok(await repository.GetAllAsync(cancellationToken)))
    .WithName("GetOffers");

app.Run();

record HealthStatus(string Status);
