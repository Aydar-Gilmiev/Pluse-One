using System.Net.Http.Json;
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
}
