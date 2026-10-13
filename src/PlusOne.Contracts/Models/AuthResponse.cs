namespace PlusOne.Contracts.Models;

public sealed class AuthResponse
{
    public bool Status { get; init; }
    public string? Message { get; init; }
    public UserDto? Data { get; init; }
}
