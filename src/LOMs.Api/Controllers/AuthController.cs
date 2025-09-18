using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Auth.Commands.ResetPassword;
using LOMs.Application.Features.Auth.Dtos;
using LOMs.Application.Features.Auth.Queries.GenerateTokens;
using LOMs.Application.Features.Auth.Queries.GetUserInfo;
using LOMs.Application.Features.Auth.Queries.RefreshTokens;
using LOMs.Contract.Requests.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LOMs.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(ICommandMediator command, IQueryMediator query, IIdentityService identityService) : ApiController
{
    private readonly ICommandMediator _command = command;
    private readonly IQueryMediator _query = query;
    private readonly IIdentityService identityService = identityService;

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginTokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [EndpointSummary("Authenticates user and returns access/refresh tokens.")]
    [EndpointDescription("Validates credentials and issues JWT tokens for authenticated sessions.")]
    public async Task<IActionResult> LoginAsync([FromBody] GenerateTokenQuery request, CancellationToken ct)
    {
        var queryModel = new GenerateTokenQuery(request.Username, request.Password);
        var result = await _query.QueryAsync(queryModel, ct);
        this.identityService.SetInsideCookie(result.Value.TokenResponse, HttpContext);
        return result.Match(
             response => Ok(new { user = response.User, refreshTokenExpiration = response.TokenResponse.ExpiresOn }),
             Problem
         );
    }

    [HttpPost("token/refresh-token")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Refreshes access token using a valid refresh token.")]
    [EndpointDescription("Exchanges an expired access token and a valid refresh token for a new token pair.")]
    public async Task<IActionResult> RefreshTokenAsync(CancellationToken ct)
    {

        HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken);

        var result = await _query.QueryAsync(new RefreshTokenQuery(refreshToken), ct);
    
        if (!result.IsError)
            this.identityService.SetInsideCookie(result.Value, HttpContext);
        return result.Match(
                _ => Ok(new { refreshTokenExpiration = result.Value.ExpiresOn }),
                Problem);
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [EndpointSummary("Logs out the user by invalidating the refresh token.")]
    [EndpointDescription("Removes the refresh token from the system to prevent further use.")]
    public async Task<IActionResult> LogoutAsync(CancellationToken ct)
    {
        HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken);
        if (!string.IsNullOrEmpty(refreshToken))
        {
            var commandModel = new LogoutCommand(refreshToken);
            var result = await _command.SendAsync(commandModel, ct);
            return result.Match(
                _ => Ok(new { Message = "Logged out successfully." }),
                Problem);
        }

        return BadRequest();
    }

    [Authorize]
    [HttpGet("current-user")]
    [ProducesResponseType(typeof(AppUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves the authenticated user's profile.")]
    [EndpointDescription("Returns user information based on the access token claims.")]
    public async Task<IActionResult> GetCurrentUserAsync(CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
            return Problem("User ID not found in token.", statusCode: StatusCodes.Status401Unauthorized);

        var queryModel = new GetUserByIdQuery(userId);
        var result = await _query.QueryAsync(queryModel, ct);
        return result.Match(Ok, Problem);
    }

    [Authorize]
    [HttpPatch("change-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Changes the authenticated user's password.")]
    [EndpointDescription("Allows the user to change their password by providing the current and new password.")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request, CancellationToken ct)
    {
        var commandModel = new ChangePasswordCommand(request.Username, request.CurrentPassword, request.NewPassword);
        var result = await _command.SendAsync(commandModel, ct);
        return result.Match(
            _ => Ok(new { Message = "Password changed successfully." }),
            Problem);
    }
}
