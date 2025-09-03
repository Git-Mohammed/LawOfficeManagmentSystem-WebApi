using LOMs.Domain.Common.Results;

namespace LOMs.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<Result<string>> GetUserNameAsync(string userId);
    Task<Result<bool>> IsInRoleAsync(string userId, string role);
    Task<Result<bool>> AuthorizeAsync(string userId, string policyName);
    Task<Result<string>> CreateUserAsync(string userName, string password);
    Task<Result<bool>> DeleteUserAsync(string userId);
}