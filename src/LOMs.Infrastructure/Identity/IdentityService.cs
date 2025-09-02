using LOMs.Application.Common.Interfaces;
using LOMs.Domain.Common.Results;

namespace LOMs.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    public Task<Result<string>> GetUserNameAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> IsInRoleAsync(string userId, string role)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> AuthorizeAsync(string userId, string policyName)
    {
        throw new NotImplementedException();
    }

    public Task<Result<string>> CreateUserAsync(string userName, string password)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> DeleteUserAsync(string userId)
    {
        throw new NotImplementedException();
    }
}