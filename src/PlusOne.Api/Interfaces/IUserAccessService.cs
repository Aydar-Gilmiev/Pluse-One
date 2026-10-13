namespace PlusOne.Api.Interfaces;

public interface IUserAccessService
{
    Task<bool> IsRegisteredUserAsync(int userId, CancellationToken cancellationToken);
}
