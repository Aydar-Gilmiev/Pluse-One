using PlusOne.Contracts.Models;

namespace PlusOne.Api.Repositories;

public interface IOfferRepository
{
    Task<IReadOnlyList<OfferDto>> GetAllAsync(
        string? search,
        bool includeExpired,
        CancellationToken cancellationToken);

    Task<OfferDto?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<OfferDto> CreateAsync(SaveOfferRequest request, CancellationToken cancellationToken);

    Task<OfferDto?> UpdateAsync(int id, SaveOfferRequest request, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
