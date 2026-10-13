using Microsoft.AspNetCore.Mvc;
using PlusOne.Api.Interfaces;
using PlusOne.Contracts.Models;

namespace PlusOne.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Route("/auth")]
    public async Task<IActionResult> Authorize(
        [FromBody] AuthRequest authRequest,
        CancellationToken cancellationToken)
    {
        return await _authService.AuthorizeAsync(authRequest, cancellationToken);
    }

    [HttpPost]
    [Route("/register")]
    public async Task<IActionResult> Register(
        [FromBody] RegRequest registerRequest,
        CancellationToken cancellationToken)
    {
        return await _authService.RegisterAsync(registerRequest, cancellationToken);
    }
}
