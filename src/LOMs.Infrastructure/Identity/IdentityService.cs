using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Auth.Dtos;
using LOMs.Domain.Common.Options;
using LOMs.Domain.Common.Results;
using LOMs.Domain.Identity; // <-- Add this for UserErrors
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LOMs.Infrastructure.Identity;

public class IdentityService(UserManager<ApplicationUser> userManager,RoleManager<ApplicationRole> _roleManager,
    IOptions<JwtOptions> jwtOptions, ILogger<IdentityService> logger) : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
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
        if (!user.EmailConfirmed)
        {
            _logger.LogWarning("Authentication failed: email not confirmed. Username: {Username}", username);
            return UserErrors.EmailNotConfirmed;
        }

        if (!await _userManager.CheckPasswordAsync(user, password))
        {
            _logger.LogWarning("Authentication failed: invalid credentials. Username: {Username}", username);
            return UserErrors.InvalidCredentials;
        }
        var roles = await _userManager.GetRolesAsync(user);
        var claims = await _userManager.GetClaimsAsync(user);
        _logger.LogInformation("Authentication successful for user: {UserId}", user.Id);
        return new AppUserDto(user.Id, user.Email!, user.UserName!, roles, claims);
    }

    public async Task<Result<TokenGenerationDto>> GenerateTokenAsync(AppUserDto user)
    {
        try
        {
            _logger.LogInformation("Generating token for user: {UserId}", user.UserId);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("username", user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            claims.AddRange(user.Claims);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key ?? throw new InvalidOperationException("JWT Key is missing.")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes);
            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: expiry,
                signingCredentials: creds
            );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = Guid.NewGuid().ToString();
            var tokenResult = new TokenGenerationDto(
                AccessToken: accessToken,
                RefreshToken: refreshToken,
                ExpiresOn: expiry
            );
            _logger.LogInformation("Token generated for user: {UserId}", user.UserId);
            return tokenResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate token for user: {UserId}", user.UserId);
            return UserErrors.TokenGenerationFailed;
        }
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
            user.RefreshToken = refreshToken;
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

    public async Task<Result<Success>> RemoveRefreshTokenAsync(string refreshToken)
    {
        _logger.LogInformation("Removing refresh token: {RefreshToken}", refreshToken);
        var user = _userManager.Users.SingleOrDefault(u => u.RefreshToken == refreshToken);
        if (user == null)
        {
            _logger.LogWarning("Remove refresh token failed: token not found. Token: {RefreshToken}", refreshToken);
            return UserErrors.InvalidRefreshToken; 
        }
        user.RefreshToken = null!;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            _logger.LogError("Remove refresh token failed for user: {UserId}", user.Id);
            return UserErrors.RefreshTokenRemoveFailed;
        }
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
        return new AppUserDto(user.Id, user.Email!, user.UserName!, roles, claims);
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
        return new AppUserDto(user.Id, user.Email!, user.UserName!, roles, claims);
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
        return new AppUserDto(user.Id, user.Email!, user.UserName!, roles, claims);
    }

    public async Task<Result<AppUserDto?>> GetUserByRefreshTokenAsync(string refreshToken)
    {
        _logger.LogInformation("Getting user by refresh token: {RefreshToken}", refreshToken);
        var user = _userManager.Users.SingleOrDefault(u => u.RefreshToken == refreshToken);
        if (user == null)
        {
            _logger.LogWarning("Get user by refresh token failed: user not found. Token: {RefreshToken}", refreshToken);
            return UserErrors.InvalidRefreshToken;
        }
        var roles = await _userManager.GetRolesAsync(user);
        var claims = await _userManager.GetClaimsAsync(user);
        return new AppUserDto(user.Id, user.Email!, user.UserName!, roles, claims);
    }

    public Task<Result<bool>> AuthorizeAsync(string userId, string policyName)
    {
        _logger.LogInformation("Authorizing user: {UserId} for policy: {PolicyName}", userId, policyName);
        throw new NotImplementedException();
    }
   
    public async Task<Result<Success>> ResetPasswordAsync(string username, string token, string newPassword)
    {
        _logger.LogInformation("Resetting password for user: {Username}", username);
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            _logger.LogWarning("Reset password failed: user not found. Username: {Username}", username);
            return UserErrors.UserNotFound;
        }
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        if (result.Succeeded)
        {
            _logger.LogInformation("Password reset successful for user: {Username}", username);
            return Result.Success;
        }
        _logger.LogWarning("Password reset failed for user: {Username}", username);
        return UserErrors.PasswordResetFailed;
    }
  
    public Task<Result<bool>> IsInRoleAsync(string userId, string role)
    {
        _logger.LogInformation("Checking if user: {UserId} is in role: {Role}", userId, role);
        throw new NotImplementedException();
    }

    public async Task<Result<string>> CreateUserAsync(string userName, string email, string password,string role)
    {
        _logger.LogInformation("Creating user: {UserName}", userName);
        var user = new ApplicationUser { UserName = userName, Email = email };
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

        if (!await _roleManager.RoleExistsAsync(role))
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
}