using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Auth.Dtos;
using LOMs.Domain.Common.Options;
using LOMs.Domain.Common.Results;
using LOMs.Domain.Identity; // <-- Add this for UserErrors
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LOMs.Infrastructure.Identity;

public class IdentityService(UserManager<ApplicationUser> userManager,RoleManager<ApplicationRole> _roleManager,
    IOptions<JwtOptions> jwtOptions, ILogger<IdentityService> logger, ITokenProvider tokenProvider, IAppDbContext context) : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IAppDbContext _context = context;
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    private readonly ILogger<IdentityService> _logger = logger;

    public async Task<Result<AppUserDto>> AuthenticateAsync(string username, string password)
    {
        _logger.LogInformation("Authenticating user: {Username}", username);
        var user = await _userManager.FindByNameAsync(username);
   
        if (user is null)
        {
            _logger.LogWarning("Authentication failed: user not found. Username: {Username}", username);
            return UserErrors.UserNotFound;
        }

        if (!await _userManager.CheckPasswordAsync(user, password))
        {
            _logger.LogWarning("Authentication failed: invalid credentials. Username: {Username}", username);
            return UserErrors.InvalidCredentials;
        }
        var roles = await _userManager.GetRolesAsync(user);
        var claims = await _userManager.GetClaimsAsync(user);
        _logger.LogInformation("Authentication successful for user: {UserId}", user.Id);
        return new AppUserDto(user.Id, user.Email!, user.UserName!, user.IsUsingTempPassword, roles, claims);
    }

    public async Task<Result<TokenResponse>> GenerateTokenAsync(AppUserDto user)

    {
        var result = await _tokenProvider.GenerateJwtTokenAsync(user);
        if (result.IsError)
        {
            _logger.LogError("Token generation failed for user: {UserId}", user.UserId);
            return result.Errors;
        }

        return result.Value;
    }

    public async Task<Result<Success>> UpdateRefreshToken(string userId, string refreshToken)
    {
        try
        {
            _logger.LogInformation("Updating refresh token for user: {UserId}", userId);
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                _logger.LogWarning("Update refresh token failed: user not found. UserId: {UserId}", userId);
                return UserErrors.UserNotFound;
            }
            //user.RefreshToken = refreshToken;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogError("Update refresh token failed for user: {UserId}", userId);
                return UserErrors.RefreshTokenUpdateFailed;
            }
            _logger.LogInformation("Refresh token updated for user: {UserId}", userId);
            return Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update refresh token for user: {UserId}", userId);
            return UserErrors.RefreshTokenUpdateFailed;
        }
    }

    public async Task<Result<Success>> RemoveRefreshTokenAsync(string refreshToken, CancellationToken ct)
    {
        _logger.LogInformation("Removing refresh token: {RefreshToken}", refreshToken);

        // 1. Find the refresh token record
        var refreshTokenEntity = await _context.RefreshTokens
            .SingleOrDefaultAsync(rt => rt.Token == refreshToken);

        if (refreshTokenEntity is null)
        {
            _logger.LogWarning("Remove refresh token failed: token not found. Token: {RefreshToken}", refreshToken);
            return UserErrors.InvalidRefreshToken;
        }

        // 2. Find the user associated with this refresh token
        var user = await _userManager.FindByIdAsync(refreshTokenEntity.UserId!);
        if (user is null)
        {
            _logger.LogWarning("Remove refresh token failed: user not found for token {RefreshToken}", refreshToken);
            return UserErrors.InvalidRefreshToken;
        }

        // 3. Remove the refresh token from DB
        _context.RefreshTokens.Remove(refreshTokenEntity);
        await _context.SaveChangesAsync(ct);

        _logger.LogInformation("Refresh token removed for user: {UserId}", user.Id);
        return Result.Success;
    }

    public async Task<Result<AppUserDto?>> GetUserByNameAsync(string username)
    {
        _logger.LogInformation("Getting user by name: {Username}", username);
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            _logger.LogWarning("Get user by name failed: user not found. Username: {Username}", username);
            return UserErrors.UserNotFound;
        }
        var roles = await _userManager.GetRolesAsync(user);
        var claims = await _userManager.GetClaimsAsync(user);
        return new AppUserDto(user.Id, user.Email!, user.UserName!, user.IsUsingTempPassword, roles, claims);
    }

    public Task<Result<string>> GetUserNameAsync(string userId)
    {
        _logger.LogInformation("Getting username for userId: {UserId}", userId);
        throw new NotImplementedException();
    }

    public async Task<Result<AppUserDto?>> GetUserByEmailAsync(string email)
    {
        _logger.LogInformation("Getting user by email: {Email}", email);
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            _logger.LogWarning("Get user by email failed: user not found. Email: {Email}", email);
            return UserErrors.UserNotFound;
        }
        var roles = await _userManager.GetRolesAsync(user);
        var claims = await _userManager.GetClaimsAsync(user);
        return new AppUserDto(user.Id, user.Email!, user.UserName!, user.IsUsingTempPassword, roles, claims);
    }

    public async Task<Result<AppUserDto?>> GetUserByIdAsync(string userId)
    {
        _logger.LogInformation("Getting user by id: {UserId}", userId);
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("Get user by id failed: user not found. UserId: {UserId}", userId);
            return UserErrors.UserNotFound;
        }
        var roles = await _userManager.GetRolesAsync(user);
        var claims = await _userManager.GetClaimsAsync(user);
        return new AppUserDto(user.Id, user.Email!, user.UserName!,user.IsUsingTempPassword, roles, claims);
    }

    public async Task<Result<AppUserDto?>> GetUserByRefreshTokenAsync(string refreshToken)
    {
        _logger.LogInformation("Retrieving user by refresh token: {RefreshToken}", refreshToken);

        var user = await _userManager.Users
            .Where(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken && rt.UserId == u.Id))
            .AsNoTracking()
            .SingleOrDefaultAsync();

        if (user is null)
        {
            _logger.LogWarning("User not found for refresh token: {RefreshToken}", refreshToken);
            return UserErrors.InvalidRefreshToken;
        }

        var roles = await _userManager.GetRolesAsync(user);
        var claims = await _userManager.GetClaimsAsync(user);

        var dto = new AppUserDto(
            user.Id,
            user.Email!,
            user.UserName!,
            user.IsUsingTempPassword,
            roles,
            claims
        );

        return dto;
    }

    public Task<Result<bool>> AuthorizeAsync(string userId, string policyName)
    {
        _logger.LogInformation("Authorizing user: {UserId} for policy: {PolicyName}", userId, policyName);
        throw new NotImplementedException();
    }

    public async Task<Result<Success>> ChangePasswordAsync(string username, string currentPassword, string newPassword)
    {
        _logger.LogInformation("Attempting password change for user: {Username}", username);

        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
        {
            _logger.LogWarning("Password change failed: user not found. Username: {Username}", username);
            return UserErrors.UserNotFound;
        }

        if (!await _userManager.CheckPasswordAsync(user, currentPassword))
        {
            _logger.LogWarning("Password change failed: invalid current password. Username: {Username}", username);
            return UserErrors.InvalidCredentials;
        }

        // Validate new password against all configured validators
        foreach (var validator in _userManager.PasswordValidators)
        {
            var validation = await validator.ValidateAsync(_userManager, user, newPassword);
            if (!validation.Succeeded)
            {
                _logger.LogWarning("Password change failed: invalid new password. Username: {Username}. Errors: {Errors}",
                    username, string.Join(", ", validation.Errors.Select(e => e.Description)));
                return UserErrors.InvalidNewPassword;
            }
        }

        var changeResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        if (!changeResult.Succeeded)
        {
            _logger.LogWarning("Password change failed for user: {Username}. Errors: {Errors}", username,
                string.Join(", ", changeResult.Errors.Select(e => $"{e.Code}: {e.Description}")));
            return UserErrors.PasswordChangeFailed;
        }

        // Mark that the user is no longer using a temporary password
        user.IsUsingTempPassword = false;
        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            _logger.LogError("Failed to persist IsUsingTempPassword flag for user: {Username}", username);
            return UserErrors.PasswordChangeFailed;
        }

        _logger.LogInformation("Password change successful for user: {Username}", username);
        return Result.Success;
    }

    public async Task<Result<Success>> ResetPasswordAsync(string username, string token, string newPassword)
    {
        _logger.LogInformation("Attempting password reset for user: {Username}", username);

        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
        {
            _logger.LogWarning("Password reset failed: user not found. Username: {Username}", username);
            return UserErrors.UserNotFound;
        }


        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        if (!result.Succeeded)
        {
            _logger.LogWarning("Password reset failed for user: {Username}", username);
            return UserErrors.PasswordResetFailed;
        }

        user.IsUsingTempPassword = false;
        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            _logger.LogError("Failed to persist IsUsingTempPassword flag for user: {Username}", username);
            return UserErrors.PasswordResetFailed;
        }

        _logger.LogInformation("Password reset successful for user: {Username}", username);
        return Result.Success;
    }


    public Task<Result<bool>> IsInRoleAsync(string userId, string role)
    {
        _logger.LogInformation("Checking if user: {UserId} is in role: {Role}", userId, role);
        throw new NotImplementedException();
    }

    public async Task<Result<string>> CreateUserAsync(string userName, string email, string password,string role)
    {
        _logger.LogInformation("Creating user: {UserName}", userName);
        var user = new ApplicationUser { UserName = userName, Email = email, IsUsingTempPassword = true };
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            _logger.LogError("User creation failed for {UserName}", userName);
            return UserErrors.UserCreationFailed;
        }

        // NOTE: If you want to assign multiple roles, change the parameter to List<string> roles and uncomment below

        //foreach (var role in roles)
        //{
        //    if (!await _roleManager.RoleExistsAsync(role))
        //    {
        //        var roleAssignResult = await _userManager.AddToRoleAsync(user, role);
        //        if (!roleAssignResult.Succeeded)
        //        {
        //            _logger.LogError("Role assignment failed for user: {UserId}", user.Id);
        //            return UserErrors.UserCreationFailed;
        //        }
        //    }
        //}

        if (await _roleManager.RoleExistsAsync(role))
        {
            var roleAssignResult = await _userManager.AddToRoleAsync(user, role);
            if (!roleAssignResult.Succeeded)
            {
                _logger.LogError("Role assignment failed for user: {UserId}", user.Id);
                return UserErrors.UserCreationFailed;
            }
        }


        _logger.LogInformation("User created and role assigned successfully: {UserId}", user.Id);
        return user.Id;
    }

    public async Task<Result<Success>> CreateRoleAsync(string roleName)
    {
        _logger.LogInformation("Creating role: {RoleName}", roleName);
        if (await _roleManager.RoleExistsAsync(roleName))
        {
            _logger.LogWarning("Role already exists: {RoleName}", roleName);
            return Result.Success;
        }

        var result = await _roleManager.CreateAsync(new ApplicationRole(roleName));
        if (result.Succeeded)
        {
            _logger.LogInformation("Role created successfully: {RoleName}", roleName);
            return Result.Success;
        }
        _logger.LogError("Role creation failed for {RoleName}: {Errors}", roleName, string.Join(", ", result.Errors.Select(e => e.Description)));
        return Error.Failure("Identity_Role_not_created", $"Role could not be created: {string.Join(", ", result.Errors.Select(e => e.Description))}");
    }

    public Task<Result<bool>> DeleteUserAsync(string userId)
    {
        _logger.LogInformation("Deleting user: {UserId}", userId);
        throw new NotImplementedException();
    }
    
    public async Task<Result<bool>> UpdateUserAsync(string userId, string userName, string email, string roles)
{
    _logger.LogInformation("Updating user {UserId}", userId);

    var user = await _userManager.FindByIdAsync(userId);
    if (user is null)
    {
        _logger.LogWarning("Update failed: user not found {UserId}", userId);
        return UserErrors.UserNotFound;
    }

    // تحديث البيانات الأساسية
    user.UserName = userName;
    user.Email = email;

    var updateResult = await _userManager.UpdateAsync(user);
    if (!updateResult.Succeeded)
    {
        _logger.LogError("Failed to update basic info for user {UserId}", userId);
        return UserErrors.UserUpdateFailed;
    }

    // تحديث الأدوار
    if (!string.IsNullOrWhiteSpace(roles))
    {
        var currentRoles = await _userManager.GetRolesAsync(user);
        var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!removeRolesResult.Succeeded)
        {
            _logger.LogError("Failed to remove old roles for user {UserId}", userId);
            return UserErrors.UserUpdateFailed;
        }

        var roleList = roles.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var role in roleList)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                _logger.LogWarning("Role {Role} does not exist, skipping", role);
                continue;
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, role);
            if (!addRoleResult.Succeeded)
            {
                _logger.LogError("Failed to assign role {Role} for user {UserId}", role, userId);
                return UserErrors.UserUpdateFailed;
            }
        }
    }

    _logger.LogInformation("User {UserId} updated successfully", userId);
    return true;
}

    public async Task<Result<Success>> BlockUserAsync(string userId)
    {
        _logger.LogInformation("Blocking user: {UserId}", userId);

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            _logger.LogWarning("Block failed: user not found {UserId}", userId);
            return UserErrors.UserNotFound;
        }

        if (!user.LockoutEnabled)
        {
            user.LockoutEnabled = true;
        }

        // نحدد نهاية القفل لفترة طويلة جدًا (مثلاً سنة) أو للأبد
        user.LockoutEnd = DateTimeOffset.MaxValue;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            _logger.LogError("Failed to block user {UserId}", userId);
            return UserErrors.UserUpdateFailed;
        }

        _logger.LogInformation("User {UserId} has been blocked successfully", userId);
        return Result.Success;
    }

    public async Task<Result<Success>> UnblockUserAsync(string userId)
    {
        _logger.LogInformation("Unblocking user: {UserId}", userId);

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            _logger.LogWarning("Unblock failed: user not found {UserId}", userId);
            return UserErrors.UserNotFound;
        }

        user.LockoutEnd = null; // فك الحظر
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            _logger.LogError("Failed to unblock user {UserId}", userId);
            return UserErrors.UserUpdateFailed;
        }

        _logger.LogInformation("User {UserId} has been unblocked successfully", userId);
        return Result.Success;
    }

}
