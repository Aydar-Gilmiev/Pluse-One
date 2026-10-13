using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlusOne.Api.Data;
using PlusOne.Api.Data.Entities;
using PlusOne.Api.Interfaces;
using PlusOne.Contracts.Models;

namespace PlusOne.Api.Services;

public sealed class AuthService : IAuthService
{
    private readonly PlusOneDbContext _contextDb;

    public AuthService(PlusOneDbContext contextDb)
    {
        _contextDb = contextDb;
    }

    public async Task<IActionResult> AuthorizeAsync(AuthRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Заполните все поля.");
        }

        var login = request.Login.Trim().ToLowerInvariant();
        var user = await _contextDb.Users
            .Include(item => item.Role)
            .FirstOrDefaultAsync(item => item.Login == login, cancellationToken);

        if (user is null || user.Password != request.Password)
        {
            return new UnauthorizedObjectResult(new AuthResponse
            {
                Status = false,
                Message = "Неверный логин или пароль."
            });
        }

        return new OkObjectResult(new AuthResponse
        {
            Status = true,
            Data = MapUser(user)
        });
    }

    public async Task<IActionResult> RegisterAsync(RegRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserName)
            || string.IsNullOrWhiteSpace(request.Login)
            || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Заполните все поля.");
        }

        var login = request.Login.Trim().ToLowerInvariant();
        var existingUser = await _contextDb.Users
            .AnyAsync(item => item.Login == login, cancellationToken);

        if (existingUser)
        {
            return new ConflictObjectResult(new AuthResponse
            {
                Status = false,
                Message = "Этот логин уже занят."
            });
        }

        var userRole = await _contextDb.Roles
            .FirstOrDefaultAsync(item => item.Name == "User", cancellationToken);

        if (userRole is null)
        {
            return new ObjectResult(new AuthResponse
            {
                Status = false,
                Message = "Не удалось определить роль пользователя."
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        var newUser = new User
        {
            UserName = request.UserName.Trim(),
            Login = login,
            Password = request.Password,
            RoleId = userRole.Id,
            Role = userRole
        };

        await _contextDb.Users.AddAsync(newUser, cancellationToken);
        await _contextDb.SaveChangesAsync(cancellationToken);

        return new OkObjectResult(new AuthResponse
        {
            Status = true,
            Data = MapUser(newUser)
        });
    }

    private static BadRequestObjectResult BadRequest(string message)
    {
        return new BadRequestObjectResult(new AuthResponse
        {
            Status = false,
            Message = message
        });
    }

    private static UserDto MapUser(User user)
    {
        return new UserDto
        {
            UserId = user.Id,
            UserName = user.UserName,
            Login = user.Login,
            Role = user.Role.Name
        };
    }
}
