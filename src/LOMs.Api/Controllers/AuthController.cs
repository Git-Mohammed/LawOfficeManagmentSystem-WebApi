using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
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
public class AuthController(ICommandMediator command, IQueryMediator query) : ApiController
{
    private readonly ICommandMediator _command = command;
    private readonly IQueryMediator _query = query;

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginTokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [EndpointSummary("Authenticates user and returns access/refresh tokens.")]
    [EndpointDescription("Validates credentials and issues JWT tokens for authenticated sessions.")]
    public async Task<IActionResult> Login([FromBody] GenerateTokenQuery request, CancellationToken ct)
    {
        var queryModel = new GenerateTokenQuery(request.Username, request.Password);
        var result = await _query.QueryAsync(queryModel, ct);
        return result.Match(Ok, Problem);
    }

    [HttpPost("token/refresh-token")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Refreshes access token using a valid refresh token.")]
    [EndpointDescription("Exchanges an expired access token and a valid refresh token for a new token pair.")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenQuery request, CancellationToken ct)
    {
        var result = await _query.QueryAsync(request, ct);
        return result.Match(Ok, Problem);
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [EndpointSummary("Logs out the user by invalidating the refresh token.")]
    [EndpointDescription("Removes the refresh token from the system to prevent further use.")]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request, CancellationToken ct)
    {
        var commandModel = new LogoutCommand(request.RefreshToken);
        var result = await _command.SendAsync(commandModel, ct);
        return result.Match(
            _ => Ok(new { Message = "Logged out successfully." }),
            Problem);
    }

    //[HttpGet("current-user/claims")]
    //[Authorize]
    //[ProducesResponseType(typeof(AppUserDto), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    //[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    //[EndpointSummary("Gets the current authenticated user's info.")]
    //[EndpointDescription("Returns user information for the currently authenticated user based on the access token.")]
    //public async Task<IActionResult> GetCurrentUserInfo(CancellationToken ct)
    //{
    //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //    var queryModel = new GetUserByIdQuery(userId);
    //    var result = await _query.QueryAsync(queryModel, ct);
    //    return result.Match(Ok, Problem);
    //}

    //[HttpPost("reset-password")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    //[EndpointSummary("Resets the user's password using a valid reset token.")]
    //[EndpointDescription("Completes the password reset flow after the user receives a token via email.")]
    //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken ct)
    //{
    //    var commandModel = new ResetPasswordCommand(request.Username, request.Token, request.NewPassword);
    //    var result = await _command.SendAsync(commandModel, ct);
    //    return result.Match(
    //        _ => Ok(new { Message = "Password reset successfully." }),
    //        Problem);
    //}
}
