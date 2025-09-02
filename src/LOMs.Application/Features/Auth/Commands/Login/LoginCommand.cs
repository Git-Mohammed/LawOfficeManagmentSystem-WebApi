using LiteBus.Commands.Abstractions;
using LOMs.Application.Features.Auth.Dtos;
using LOMs.Application.Features.Customers.Dtos;
using LOMs.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Application.Features.Auth.Commands.Login;

public sealed record LoginCommand(
    string Email,
    string Password
) : ICommand<Result<LoginDto>>;