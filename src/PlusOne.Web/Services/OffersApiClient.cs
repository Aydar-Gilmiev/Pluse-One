using System.Net.Http.Json;
using System.Net;
using PlusOne.Contracts.Models;

namespace PlusOne.Web.Services;

public sealed class OffersApiClient(HttpClient httpClient)
{
    private const string OffersEndpoint = "api/offers";

    public async Task<IReadOnlyList<OfferDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var offers = await httpClient.GetFromJsonAsync<List<OfferDto>>(OffersEndpoint, cancellationToken);
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
}
