using LOMs.Application.Features.Auth.Dtos;
using LOMs.Domain.Common.Results;
using Microsoft.AspNetCore.Http;

namespace LOMs.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<Result<string>> GetUserNameAsync(string userId);
    Task<Result<AppUserDto?>> GetUserByEmailAsync(string email);
    Task<Result<AppUserDto?>> GetUserByNameAsync(string username); // New: For username-based lookup
    Task<Result<AppUserDto?>> GetUserByIdAsync(string userId);
    Task<Result<AppUserDto?>> GetUserByRefreshTokenAsync(string refreshToken);
    Task<Result<AppUserDto>> AuthenticateAsync(string username, string password); // Changed: Use username
    Task<Result<Success>> UpdateRefreshToken(string userId, string refreshToken);
    Task<Result<Success>> RemoveRefreshTokenAsync(string refreshToken, CancellationToken ct);
    Task<Result<TokenResponse>> GenerateTokenAsync(AppUserDto user);
    Task<Result<bool>> IsInRoleAsync(string userId, string role);
    Task<Result<Success>> CreateRoleAsync(string roleName);
    Task<Result<bool>> AuthorizeAsync(string userId, string policyName);
    Task<Result<Success>> ResetPasswordAsync(string username, string token, string newPassword);
    Task<Result<Success>> ChangePasswordAsync(string username, string currentPassword, string newPassword);

    Task<Result<string>> CreateUserAsync(string userName, string email, string password, string roles);
    Task<Result<bool>> DeleteUserAsync(string userId);

    void SetInsideCookie(TokenResponse tokenResponse, HttpContext context);
    // Add for reset password (below)
}