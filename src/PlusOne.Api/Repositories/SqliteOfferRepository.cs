using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PlusOne.Api.Data;
using PlusOne.Api.Data.Entities;
using PlusOne.Contracts.Models;

namespace PlusOne.Api.Repositories;

public sealed class SqliteOfferRepository(PlusOneDbContext database) : IOfferRepository
{
    private static readonly Expression<Func<Offer, OfferDto>> OfferProjection = offer => new OfferDto
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
    };

    public async Task<IReadOnlyList<OfferDto>> GetAllAsync(
        string? search,
        bool includeExpired,
        CancellationToken cancellationToken)
    {
        var query = database.Offers.AsNoTracking();

        if (!includeExpired)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            query = query.Where(offer => offer.ValidUntil == null || offer.ValidUntil >= today);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var pattern = $"%{search.Trim()}%";
            query = query.Where(offer =>
                EF.Functions.Like(offer.Title, pattern) ||
                EF.Functions.Like(offer.CompanyName, pattern));
        }

        return await query
            .OrderBy(offer => offer.Id)
            .Select(OfferProjection)
            .ToListAsync(cancellationToken);
    }

    public async Task<OfferDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await database.Offers
            .AsNoTracking()
            .Where(offer => offer.Id == id)
            .Select(OfferProjection)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<OfferDto> CreateAsync(
        SaveOfferRequest request,
        CancellationToken cancellationToken)
    {
        var offer = new Offer();
        ApplyRequest(offer, request);

        database.Offers.Add(offer);
        await database.SaveChangesAsync(cancellationToken);
        return MapToDto(offer);
    }

    public async Task<OfferDto?> UpdateAsync(
        int id,
        SaveOfferRequest request,
        CancellationToken cancellationToken)
    {
        var offer = await database.Offers.FindAsync([id], cancellationToken);
        if (offer is null)
        {
            return null;
        }

        ApplyRequest(offer, request);
        await database.SaveChangesAsync(cancellationToken);
        return MapToDto(offer);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var offer = await database.Offers.FindAsync([id], cancellationToken);
        if (offer is null)
        {
            return false;
        }

        database.Offers.Remove(offer);
        await database.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static void ApplyRequest(Offer offer, SaveOfferRequest request)
    {
        offer.CompanyName = request.CompanyName.Trim();
        offer.Title = request.Title.Trim();
        offer.Description = request.Description.Trim();
        offer.Category = request.Category.Trim();
        offer.SourceType = request.SourceType;
        offer.SourceUrl = request.SourceUrl.Trim();
        offer.ValidUntil = request.ValidUntil;
        offer.PromoCode = NormalizeOptional(request.PromoCode);
        offer.RedemptionInstructions = NormalizeOptional(request.RedemptionInstructions);
    }

    private static OfferDto MapToDto(Offer offer) => new()
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
    };

    private static string? NormalizeOptional(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
