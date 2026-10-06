using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PlusOne.Api.Repositories;
using PlusOne.Contracts.Models;
using Xunit;

namespace PlusOne.Api.Tests;

public sealed class OffersEndpointTests
{
    [Fact]
    public async Task GetOffers_ReturnsSeededOffersWithExpectedFields()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        using var response = await client.GetAsync("/api/offers");
        var payload = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(payload);
        var offers = JsonSerializer.Deserialize<OfferDto[]>(
            payload,
            new JsonSerializerOptions(JsonSerializerDefaults.Web));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(JsonValueKind.Array, document.RootElement.ValueKind);
        Assert.NotNull(offers);
        Assert.True(offers.Length >= 6);
        var expectedProperties = new[]
        {
            "id", "companyName", "title", "description", "category", "sourceType", "sourceUrl", "validUntil"
        };
        foreach (var offer in document.RootElement.EnumerateArray())
        {
            foreach (var property in expectedProperties)
            {
                Assert.True(offer.TryGetProperty(property, out _), $"Missing JSON property: {property}");
            }
        }

        Assert.All(offers, offer =>
        {
            Assert.NotEqual(0, offer.Id);
            Assert.False(string.IsNullOrWhiteSpace(offer.CompanyName));
            Assert.False(string.IsNullOrWhiteSpace(offer.Title));
            Assert.False(string.IsNullOrWhiteSpace(offer.Description));
            Assert.False(string.IsNullOrWhiteSpace(offer.Category));
            Assert.False(string.IsNullOrWhiteSpace(offer.SourceUrl));
        });
    }

    [Fact]
    public async Task GetOffers_WhenRepositoryIsEmpty_ReturnsEmptyArray()
    {
        await using var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IOfferRepository>();
                services.AddSingleton<IOfferRepository>(new EmptyOfferRepository());
            }));
        using var client = factory.CreateClient();

        using var response = await client.GetAsync("/api/offers");
        var offers = await response.Content.ReadFromJsonAsync<OfferDto[]>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(offers);
        Assert.Empty(offers);
    }

    [Fact]
    public async Task GetOffers_AllowsConfiguredBlazorOrigin()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var request = new HttpRequestMessage(HttpMethod.Options, "/api/offers");
        request.Headers.Add("Origin", "http://localhost:5112");
        request.Headers.Add("Access-Control-Request-Method", "GET");

        using var response = await factory.CreateClient().SendAsync(request);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal("http://localhost:5112", response.Headers
            .GetValues("Access-Control-Allow-Origin").Single());
    }

    [Fact]
    public async Task Swagger_ContainsOffersEndpoint()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        using var response = await client.GetAsync("/swagger/v1/swagger.json");
        var swaggerDocument = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("/api/offers", swaggerDocument);
    }

    private sealed class EmptyOfferRepository : IOfferRepository
    {
        public Task<IReadOnlyList<OfferDto>> GetAllAsync(CancellationToken cancellationToken) =>
            Task.FromResult<IReadOnlyList<OfferDto>>([]);
    }
}
