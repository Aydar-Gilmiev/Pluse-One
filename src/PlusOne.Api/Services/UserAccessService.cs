using Microsoft.EntityFrameworkCore;
using PlusOne.Api.Data;
using PlusOne.Api.Interfaces;

namespace PlusOne.Api.Services;

public sealed class UserAccessService : IUserAccessService
{
    private readonly PlusOneDbContext _contextDb;

    public UserAccessService(PlusOneDbContext contextDb)
    {
        _contextDb = contextDb;
    }

    public Task<bool> IsRegisteredUserAsync(int userId, CancellationToken cancellationToken)
    {
        return _contextDb.Users.AnyAsync(item => item.Id == userId, cancellationToken);
    }
}
