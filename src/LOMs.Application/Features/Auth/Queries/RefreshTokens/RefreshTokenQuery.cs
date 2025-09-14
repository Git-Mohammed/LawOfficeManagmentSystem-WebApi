
using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Auth.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Auth.Queries.RefreshTokens;

public record RefreshTokenQuery(string RefreshToken, string ExpiredAccessToken) : IQuery<Result<TokenResponse>>;