using LiteBus.Commands.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Auth.Dtos;
using LOMs.Application.Features.Customers.Commands.CreateCustomer;
using LOMs.Application.Features.Customers.Dtos;
using LOMs.Domain.Common.Results;
using LOMs.Domain.Customers;
using LOMs.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Application.Features.Auth.Commands.Login
{
    //public class LoginCommandHandler(ILogger<LoginCommandHandler> logger,
    //IAppDbContext context,
    //IMapper mapper)
    //: ICommandHandler<LoginCommand, Result<TokenDto>>
    //{
    //    private readonly ILogger<LoginCommandHandler> _logger = logger;
    //    private readonly IAppDbContext _context = context;

    //    public async Task<Result<TokenDto>> HandleAsync(LoginCommand command, CancellationToken ct)
    //    {
    //        var email = command.Email.Trim().ToLower();

    //        // TODO add index on email
    //        var exists = await _context.Users.AnyAsync(
    //            u => u.Email!.ToLower() == email,
    //            ct);

    //        if (exists)
    //        {
    //            _logger.LogWarning("User not exists.");
    //            // TODO add errors
    //            //return UserErrors.CustomerExists;
    //        }


    //        // Login
            

    //        _logger.LogInformation("User logged in successfully. Id: {UserId}", Id);
    //    }
    //}
}
