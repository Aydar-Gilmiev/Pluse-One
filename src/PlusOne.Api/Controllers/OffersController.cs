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

    [HttpGet("{id:int}")]
    [ProducesResponseType<OfferDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OfferDto>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var offer = await repository.GetByIdAsync(id, cancellationToken);
        return offer is null ? NotFound() : Ok(offer);
    }
}
