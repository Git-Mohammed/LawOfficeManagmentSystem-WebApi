

using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Auth.Dtos;
using LOMs.Domain.Common.Results;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Auth.Queries.GenerateTokens;

public class GenerateTokenQueryHandler(ILogger<GenerateTokenQueryHandler> logger, IIdentityService identityService, ITokenProvider tokenProvider)
    : IQueryHandler<GenerateTokenQuery, Result<LoginTokenDto>>
{
    private readonly ILogger<GenerateTokenQueryHandler> _logger = logger;
    private readonly IIdentityService _identityService = identityService;
    private readonly ITokenProvider _tokenProvider = tokenProvider;

    public async Task<Result<LoginTokenDto>> HandleAsync(GenerateTokenQuery query, CancellationToken ct)
    {
        var userResponse = await _identityService.AuthenticateAsync(query.Username, query.Password);

        if (userResponse.IsError)
        {
            return userResponse.Errors;
        }

        var generateTokenResult = await _tokenProvider.GenerateJwtTokenAsync(userResponse.Value, ct);

        if (generateTokenResult.IsError)
        {
            _logger.LogError("Generate token error occurred: {ErrorDescription}", generateTokenResult.TopError.Description);

            return generateTokenResult.Errors;
        }

        return  new LoginTokenDto(userResponse.Value,generateTokenResult.Value);
    }
}