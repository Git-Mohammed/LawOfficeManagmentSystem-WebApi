using LiteBus.Commands.Abstractions;
using LiteBus.Events.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Auth.Dtos;
using LOMs.Domain.Common.Results;
using LOMs.Domain.Identity.DomainEvents;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Auth.Commands.Refresh
{
    public class RefreshCommandHandler : ICommandHandler<RefreshCommand, Result<TokenGenerationDto>>
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<RefreshCommandHandler> _logger;
        private readonly IDomainEventPublisher _eventPublisher;

        public RefreshCommandHandler(IIdentityService identityService, ILogger<RefreshCommandHandler> logger, IDomainEventPublisher eventPublisher)
        {
            _identityService = identityService;
            _logger = logger;
            _eventPublisher = eventPublisher;
        }

        public async Task<Result<TokenGenerationDto>> HandleAsync(RefreshCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Refresh token attempt: {RefreshToken}", command.RefreshToken);
            var userResult = await _identityService.GetUserByRefreshTokenAsync(command.RefreshToken);
            if (userResult.IsError || userResult.Value is null)
            {
                _logger.LogWarning("Refresh failed: invalid refresh token {RefreshToken}", command.RefreshToken);
                return userResult.Errors; 
            }

            var tokenResult = await _identityService.GenerateTokenAsync(userResult.Value);
            if (tokenResult.IsError)
            {
                _logger.LogError("Refresh failed: token generation error for user {UserId}", userResult.Value.UserId);
                return tokenResult.Errors;  
            }

            var storeResult = await _identityService.UpdateRefreshToken(userResult.Value.UserId, tokenResult.Value.RefreshToken!);
            if (storeResult.IsError)
            {
                _logger.LogError("Refresh failed: could not update refresh token for user {UserId}", userResult.Value.UserId);
                return storeResult.Errors;  
            }

            var user = userResult.Value;
            await _eventPublisher.PublishAsync(new UserTokenRefreshedEvent(user!.UserId, user.Username, DateTime.UtcNow),cancellationToken);

            _logger.LogInformation("Refresh successful for user {UserId}", userResult.Value.UserId);
            return tokenResult;
        }
    }
}