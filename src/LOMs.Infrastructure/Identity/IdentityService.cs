using LOMs.Application.Common.Interfaces;
using LOMs.Domain.Common.Results;
using Microsoft.AspNetCore.Identity;

namespace LOMs.Infrastructure.Identity;

public class IdentityService(UserManager<ApplicationUser> userManager) : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

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

    public async Task<Result<string>> CreateUserAsync(string userName, string password)
    {
        var user = new ApplicationUser { UserName = userName, Email = userName };
        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded)
            return user.Id;
        
        return Error.Failure("Identity_User_not_created", result.Errors.First().Description);
    }

    public Task<Result<bool>> DeleteUserAsync(string userId)
    {
        throw new NotImplementedException();
    }
}