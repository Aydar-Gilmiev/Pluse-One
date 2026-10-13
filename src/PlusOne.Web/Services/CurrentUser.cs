using PlusOne.Contracts.Models;

namespace PlusOne.Web.Services;

public sealed class CurrentUser
{
    public UserDto? User { get; private set; }

    public bool IsAuthorized => User is not null;

    public bool IsAdmin => User?.Role == "Admin";

    public event Action? Changed;

    public void SetUser(UserDto user)
    {
        User = user;
        Changed?.Invoke();
    }

    public void Logout()
    {
        User = null;
        Changed?.Invoke();
    }
}
