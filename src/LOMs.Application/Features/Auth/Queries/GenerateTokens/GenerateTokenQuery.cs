using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Auth.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Auth.Queries.GenerateTokens;

public record GenerateTokenQuery(
    string Username,
    string Password) : IQuery<Result<LoginTokenDto>>;