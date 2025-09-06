using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace LOMs.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(ICommandMediator command, IQueryMediator query) : ApiController 
    {

        //[HttpPost("/login")]
        //public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
        //{
        //   // send command

        //    // return response
        //}

        //[HttpPost("/register")]
        //public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
        //{
        //    // send command

        //    // return response
        //}

        //[HttpPost("/logout")]
        //public async Task<IActionResult> Logout( CancellationToken ct)
        //{
        //    // send command

        //    // return response
        //}

        //[HttpPost("/refresh")]
        //public async Task<IActionResult> Refresh([FromBody] RefreshRequest request, CancellationToken ct)
        //{
        //    // send command

        //    // return response
        //}
    }
}
