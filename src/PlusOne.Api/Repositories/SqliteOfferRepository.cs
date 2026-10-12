using Microsoft.EntityFrameworkCore;
using PlusOne.Api.Data;
using PlusOne.Contracts.Models;

namespace PlusOne.Api.Repositories;

public sealed class SqliteOfferRepository(PlusOneDbContext database) : IOfferRepository
{
    public async Task<IReadOnlyList<OfferDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await database.Offers
            .AsNoTracking()
            .OrderBy(offer => offer.Id)
            .Select(offer => new OfferDto
            {
                Id = offer.Id,
                CompanyName = offer.CompanyName,
                Title = offer.Title,
                Description = offer.Description,
                Category = offer.Category,
                SourceType = offer.SourceType,
                SourceUrl = offer.SourceUrl,
                ValidUntil = offer.ValidUntil,
                PromoCode = offer.PromoCode,
                RedemptionInstructions = offer.RedemptionInstructions
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<OfferDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await database.Offers
            .AsNoTracking()
            .Where(offer => offer.Id == id)
            .Select(offer => new OfferDto
            {
                Id = offer.Id,
                CompanyName = offer.CompanyName,
                Title = offer.Title,
                Description = offer.Description,
                Category = offer.Category,
                SourceType = offer.SourceType,
                SourceUrl = offer.SourceUrl,
                ValidUntil = offer.ValidUntil,
                PromoCode = offer.PromoCode,
                RedemptionInstructions = offer.RedemptionInstructions
            })
            .SingleOrDefaultAsync(cancellationToken);
    }
}
