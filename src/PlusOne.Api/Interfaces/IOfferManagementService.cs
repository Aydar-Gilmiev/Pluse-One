using Microsoft.AspNetCore.Mvc;
using PlusOne.Contracts.Models;

namespace PlusOne.Api.Interfaces;

public interface IOfferManagementService
{
    Task<IActionResult> CreateOfferAsync(SaveOfferRequest request, int userId, CancellationToken cancellationToken);
    Task<IActionResult> UpdateOfferAsync(int offerId, SaveOfferRequest request, int userId, CancellationToken cancellationToken);
    Task<IActionResult> DeleteOfferAsync(int offerId, int userId, CancellationToken cancellationToken);
}
