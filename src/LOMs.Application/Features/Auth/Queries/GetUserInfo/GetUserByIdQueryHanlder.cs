using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Auth.Dtos;
using LOMs.Domain.Common.Results;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Auth.Queries.GetUserInfo;

public class GetUserByIdQueryHanlder(ILogger<GetUserByIdQueryHanlder> logger, IIdentityService identityService)
    : IQueryHandler<GetUserByIdQuery, Result<AppUserDto>>
{
    private readonly ILogger<GetUserByIdQueryHanlder> _logger = logger;
    private readonly IIdentityService _identityService = identityService;

    public async Task<Result<AppUserDto>> HandleAsync(GetUserByIdQuery request, CancellationToken ct)
    {
        var getUserByIdResult = await _identityService.GetUserByIdAsync(request.UserId!);

        if (getUserByIdResult.IsError)
        {
            _logger.LogError("User with Id { UserId }{ErrorDetails}", request.UserId, getUserByIdResult.TopError.Description);

            return getUserByIdResult.Errors;
        }

        return getUserByIdResult.Value;
    }
}