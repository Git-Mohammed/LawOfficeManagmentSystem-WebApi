using System.Security.Claims;
using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Errors;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Auth.Dtos;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Auth.Queries.RefreshTokens;

public class RefreshTokenQueryHandler(ILogger<RefreshTokenQueryHandler> logger, IIdentityService identityService, IAppDbContext context, ITokenProvider tokenProvider)
    : IQueryHandler<RefreshTokenQuery, Result<TokenResponse>>
{
    private readonly ILogger<RefreshTokenQueryHandler> _logger = logger;
    private readonly IIdentityService _identityService = identityService;
    private readonly IAppDbContext _context = context;
    private readonly ITokenProvider _tokenProvider = tokenProvider;

    public async Task<Result<TokenResponse>> HandleAsync(RefreshTokenQuery request, CancellationToken ct)
    {
        //var principal = _tokenProvider.GetPrincipalFromExpiredToken(request.ExpiredAccessToken);

        //if (principal is null)
        //{
        //    _logger.LogError("Expired access token is not valid");

        //    return ApplicationErrors.ExpiredAccessTokenInvalid;
        //}

        //var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //if (userId is null)
        //{
        //    _logger.LogError("Invalid userId claim");

        //    return ApplicationErrors.UserIdClaimInvalid;
        //}

        var getUserResult = await _identityService.GetUserByRefreshTokenAsync(request.RefreshToken);

        if (getUserResult.IsError)
        {
            _logger.LogError("Get user by refreshToken error occurred: {ErrorDescription}", getUserResult.TopError.Description);
            return getUserResult.Errors;
        }

        //var refreshToken = await _context.RefreshTokens.Include(r => r.user).FirstOrDefaultAsync(r => r.Token == request.RefreshToken, ct);

        //if (refreshToken is null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
        //{
        //    _logger.LogError("Refresh token has expired");

        //    return ApplicationErrors.RefreshTokenExpired;
        //}

        var generateTokenResult = await _tokenProvider.GenerateJwtTokenAsync(getUserResult!.Value!, ct);

        if (generateTokenResult.IsError)
        {
            _logger.LogError("Generate token error occurred: {ErrorDescription}", generateTokenResult.TopError.Description);

            return generateTokenResult.Errors;
        }

        return generateTokenResult.Value;
    }
}