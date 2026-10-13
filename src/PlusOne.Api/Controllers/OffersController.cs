using PlusOne.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PlusOne.Api.Repositories;
using PlusOne.Contracts.Models;

namespace PlusOne.Api.Controllers;

[ApiController]
[Route("api/offers")]
public sealed class OffersController : ControllerBase
{
    private readonly IOfferRepository _repository;
    private readonly IOfferManagementService _offerManagementService;

    public OffersController(
        IOfferRepository repository,
        IOfferManagementService offerManagementService)
    {
        _repository = repository;
        _offerManagementService = offerManagementService;
    }

    [HttpGet]
    [ProducesResponseType<IReadOnlyList<OfferDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<OfferDto>>> GetAll(
        CancellationToken cancellationToken,
        [FromQuery] string? search = null,
        [FromQuery] bool includeExpired = false)
    {
        var offers = await _repository.GetAllAsync(search, includeExpired, cancellationToken);
        return Ok(offers);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<OfferDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OfferDto>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var offer = await _repository.GetByIdAsync(id, cancellationToken);
        return offer is null ? NotFound() : Ok(offer);
    }

    [HttpPost]
    [ProducesResponseType<OfferDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create(
        [FromBody] SaveOfferRequest request,
        [FromQuery] int userId,
        CancellationToken cancellationToken)
    {
        return await _offerManagementService.CreateOfferAsync(request, userId, cancellationToken);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType<OfferDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] SaveOfferRequest request,
        [FromQuery] int userId,
        CancellationToken cancellationToken)
    {
        return await _offerManagementService.UpdateOfferAsync(id, request, userId, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(
        int id,
        [FromQuery] int userId,
        CancellationToken cancellationToken)
    {
        return await _offerManagementService.DeleteOfferAsync(id, userId, cancellationToken);
    }
}
