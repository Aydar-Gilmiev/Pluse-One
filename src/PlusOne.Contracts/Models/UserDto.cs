namespace PlusOne.Contracts.Models;

public sealed class UserDto
{
    public int UserId { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string Login { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
}
