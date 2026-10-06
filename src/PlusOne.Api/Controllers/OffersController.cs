using Microsoft.AspNetCore.Mvc;
using PlusOne.Api.Repositories;
using PlusOne.Contracts.Models;

namespace PlusOne.Api.Controllers;

[ApiController]
[Route("api/offers")]
public sealed class OffersController(IOfferRepository repository) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<OfferDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<OfferDto>>> GetAll(
        CancellationToken cancellationToken)
    {
        var offers = await repository.GetAllAsync(cancellationToken);
        return Ok(offers);
    }
}
