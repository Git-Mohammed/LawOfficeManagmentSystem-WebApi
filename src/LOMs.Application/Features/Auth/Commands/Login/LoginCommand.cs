using LiteBus.Commands.Abstractions;
using LOMs.Application.Features.Auth.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Auth.Commands.Login;

public sealed record LoginCommand(
    string Username,
    string Password
) : ICommand<Result<TokenDto>>;