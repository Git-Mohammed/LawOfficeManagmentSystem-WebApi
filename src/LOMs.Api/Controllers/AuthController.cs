using LiteBus.Commands.Abstractions;
using LOMs.Application.Features.Auth.Commands.Login;
using LOMs.Application.Features.Auth.Commands.Refresh;
using LOMs.Application.Features.Auth.Commands.ResetPassword;
using LOMs.Contract.Requests.Auth;
using Microsoft.AspNetCore.Mvc;

namespace LOMs.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(ICommandMediator command) : ApiController 
    {

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
        {
            var loginCommand = new LoginCommand(request.Username, request.Password);

            var result = await command.SendAsync(loginCommand, ct);

            return result.Match(
                response => Ok(response),
                Problem);
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request, CancellationToken ct)
        {
            var refreshCommand = new RefreshCommand(request.RefreshToken);

            var result = await command.SendAsync(refreshCommand, ct);

            return result.Match(
                response => Ok(response),
                Problem);
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshRequest request, CancellationToken ct)
        {
            var logoutCommand = new LogoutCommand(request.RefreshToken);
            var result = await command.SendAsync(logoutCommand, ct);

            return result.Match(
                _ => Ok(new { Message = "Logged out successfully." }),
                Problem);
        }


        // LETER: API for sending the email when user requests a password reset is not implemented.

        //[HttpPost("reset-password")]
        //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken ct)
        //{
        //    var resetCommand = new ResetPasswordCommand(request.Username, request.Token, request.NewPassword);

        //    var result = await command.SendAsync(resetCommand, ct);

        //    return result.Match(
        //        _ => Ok(new { Message = "Password reset successfully." }),
        //        Problem);
        //}
    }
}
