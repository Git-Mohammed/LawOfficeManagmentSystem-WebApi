
using LiteBus.Commands.Abstractions;
using LOMs.Application.Features.Auth.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Auth.Commands.Refresh;

public sealed record RefreshCommand(string RefreshToken) : ICommand<Result<TokenGenerationDto>>;

