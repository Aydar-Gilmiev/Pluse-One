using System.Net.Http.Json;
using System.Net;
using PlusOne.Contracts.Models;

namespace PlusOne.Web.Services;

public sealed class OffersApiClient(HttpClient httpClient)
{
    private const string OffersEndpoint = "api/offers";

    public async Task<IReadOnlyList<OfferDto>> GetAllAsync(
        CancellationToken cancellationToken,
        string? search = null,
        bool includeExpired = false)
    {
        var parameters = new List<string>();
        if (!string.IsNullOrWhiteSpace(search))
        {
            parameters.Add($"search={Uri.EscapeDataString(search.Trim())}");
        }

        if (includeExpired)
        {
            parameters.Add("includeExpired=true");
        }

        var endpoint = parameters.Count == 0
            ? OffersEndpoint
            : $"{OffersEndpoint}?{string.Join('&', parameters)}";
        var offers = await httpClient.GetFromJsonAsync<List<OfferDto>>(endpoint, cancellationToken);
        return offers ?? [];
    }

    public async Task<OfferDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync($"api/offers/{id}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<OfferDto>(cancellationToken);
    }

    public async Task<OfferDto> CreateAsync(
        SaveOfferRequest request,
        CancellationToken cancellationToken)
    {
        using var response = await httpClient.PostAsJsonAsync(OffersEndpoint, request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<OfferDto>(cancellationToken)
            ?? throw new InvalidOperationException("API returned an empty response after creating an offer.");
    }

    public async Task<OfferDto> UpdateAsync(
        int id,
        SaveOfferRequest request,
        CancellationToken cancellationToken)
    {
        using var response = await httpClient.PutAsJsonAsync($"{OffersEndpoint}/{id}", request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<OfferDto>(cancellationToken)
            ?? throw new InvalidOperationException("API returned an empty response after updating an offer.");
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        using var response = await httpClient.DeleteAsync($"{OffersEndpoint}/{id}", cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}
