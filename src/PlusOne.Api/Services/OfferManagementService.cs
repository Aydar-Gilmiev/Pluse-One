using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlusOne.Api.Data;
using PlusOne.Api.Interfaces;
using PlusOne.Api.Repositories;
using PlusOne.Contracts.Models;

namespace PlusOne.Api.Services;

public sealed class OfferManagementService : IOfferManagementService
{
    private readonly PlusOneDbContext _contextDb;
    private readonly IOfferRepository _offerRepository;

    public OfferManagementService(PlusOneDbContext contextDb, IOfferRepository offerRepository)
    {
        _contextDb = contextDb;
        _offerRepository = offerRepository;
    }

    public async Task<IActionResult> CreateOfferAsync(
        SaveOfferRequest request,
        int userId,
        CancellationToken cancellationToken)
    {
        var accessResult = await CheckAdminAccessAsync(userId, cancellationToken);
        if (accessResult is not null)
        {
            return accessResult;
        }

        var offer = await _offerRepository.CreateAsync(request, cancellationToken);
        return new OkObjectResult(offer);
    }

    public async Task<IActionResult> UpdateOfferAsync(
        int offerId,
        SaveOfferRequest request,
        int userId,
        CancellationToken cancellationToken)
    {
        var accessResult = await CheckAdminAccessAsync(userId, cancellationToken);
        if (accessResult is not null)
        {
            return accessResult;
        }

        var offer = await _offerRepository.UpdateAsync(offerId, request, cancellationToken);
        if (offer is null)
        {
            return new NotFoundObjectResult(new { status = false, message = "Предложение не найдено." });
        }

        return new OkObjectResult(offer);
    }

    public async Task<IActionResult> DeleteOfferAsync(
        int offerId,
        int userId,
        CancellationToken cancellationToken)
    {
        var accessResult = await CheckAdminAccessAsync(userId, cancellationToken);
        if (accessResult is not null)
        {
            return accessResult;
        }

        var deleted = await _offerRepository.DeleteAsync(offerId, cancellationToken);
        if (!deleted)
        {
            return new NotFoundObjectResult(new { status = false, message = "Предложение не найдено." });
        }

        return new OkObjectResult(new { status = true });
    }

    private async Task<IActionResult?> CheckAdminAccessAsync(int userId, CancellationToken cancellationToken)
    {
        var role = await _contextDb.Users
            .Where(item => item.Id == userId)
            .Select(item => item.Role.Name)
            .FirstOrDefaultAsync(cancellationToken);

        if (role is null)
        {
            return new UnauthorizedObjectResult(new { status = false, message = "Пользователь не авторизован." });
        }

        if (role != "Admin")
        {
            return new ObjectResult(new { status = false, message = "Недостаточно прав." })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }

        return null;
    }
}
