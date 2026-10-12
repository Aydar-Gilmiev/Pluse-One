using PlusOne.Contracts.Models;

namespace PlusOne.Api.Repositories;

public interface IOfferRepository
{
    Task<IReadOnlyList<OfferDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<OfferDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
}
