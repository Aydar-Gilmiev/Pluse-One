using System.Net.Http.Json;
using System.Text.Json;
using PlusOne.Contracts.Models;

namespace PlusOne.Web.Services;

public sealed class AuthApiClient
{
    private readonly HttpClient _httpClient;
    public AuthApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<AuthResponse> Authorize(AuthRequest authRequest)
    {
        return SendAsync("auth", authRequest);
    }

    public Task<AuthResponse> Register(RegRequest regRequest)
    {
        return SendAsync("register", regRequest);
    }

    private async Task<AuthResponse> SendAsync<TRequest>(string endpoint, TRequest request)
    {
        using var response = await _httpClient.PostAsJsonAsync(endpoint, request);
        var content = await response.Content.ReadAsStringAsync();

        using var document = JsonDocument.Parse(content);
        var root = document.RootElement;
        var status = root.TryGetProperty("status", out var statusValue)
            && statusValue.ValueKind is JsonValueKind.True or JsonValueKind.False
            && statusValue.GetBoolean();
        var message = root.TryGetProperty("message", out var messageValue)
            && messageValue.ValueKind == JsonValueKind.String
            ? messageValue.GetString()
            : null;

        if (!status || !root.TryGetProperty("data", out var dataValue))
        {
            return new AuthResponse
            {
                Status = status,
                Message = message ?? "Не удалось выполнить запрос."
            };
        }

        return new AuthResponse
        {
            Status = true,
            Message = message,
            Data = new UserDto
            {
                UserId = dataValue.GetProperty("userId").GetInt32(),
                UserName = dataValue.GetProperty("userName").GetString() ?? string.Empty,
                Login = dataValue.GetProperty("login").GetString() ?? string.Empty,
                Role = dataValue.GetProperty("role").GetString() ?? string.Empty
            }
        };
    }
}
