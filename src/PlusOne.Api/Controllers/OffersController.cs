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
        CancellationToken cancellationToken,
        [FromQuery] string? search = null,
        [FromQuery] bool includeExpired = false)
    {
        var offers = await repository.GetAllAsync(search, includeExpired, cancellationToken);
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

    [HttpPost]
    [ProducesResponseType<OfferDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OfferDto>> Create(
        [FromBody] SaveOfferRequest request,
        CancellationToken cancellationToken)
    {
        var offer = await repository.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = offer.Id }, offer);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType<OfferDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OfferDto>> Update(
        int id,
        [FromBody] SaveOfferRequest request,
        CancellationToken cancellationToken)
    {
        var offer = await repository.UpdateAsync(id, request, cancellationToken);
        return offer is null ? NotFound() : Ok(offer);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await repository.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
