using Microsoft.AspNetCore.Mvc;
using PlusOne.Contracts.Models;

namespace PlusOne.Api.Interfaces;

public interface IAuthService
{
    Task<IActionResult> AuthorizeAsync(AuthRequest request, CancellationToken cancellationToken);
    Task<IActionResult> RegisterAsync(RegRequest request, CancellationToken cancellationToken);
}
