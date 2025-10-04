using LOMs.Application.Features.Auth.Dtos;
using LOMs.Domain.Common.Results;
using System.Security.Claims;

namespace LOMs.Application.Common.Interfaces;

public interface ITokenProvider
{
    Task<Result<TokenResponse>> GenerateJwtTokenAsync(AppUserDto user, CancellationToken ct = default);

    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
