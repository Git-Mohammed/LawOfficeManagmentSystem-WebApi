using LiteBus.Commands.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Domain.Common.Results;
using LOMs.Domain.Identity.DomainEvents;
using Microsoft.Extensions.Logging;

public class LogoutCommandHandler : ICommandHandler<LogoutCommand, Result<Success>>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<LogoutCommandHandler> _logger;
    private readonly IDomainEventPublisher _eventPublisher;

    public LogoutCommandHandler(
        IIdentityService identityService,
        ILogger<LogoutCommandHandler> logger,
        IDomainEventPublisher eventPublisher)
    {
        _identityService = identityService;
        _logger = logger;
        _eventPublisher = eventPublisher;
    }

    public async Task<Result<Success>> HandleAsync(LogoutCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Logout attempt with refresh token: {RefreshToken}", command.RefreshToken);

        var userResult = await _identityService.GetUserByRefreshTokenAsync(command.RefreshToken);
        if (userResult.IsError)
        {
            _logger.LogWarning("Logout failed: user not found. Refresh token: {RefreshToken}", command.RefreshToken);
            return userResult.Errors;
        }

        var result = await _identityService.RemoveRefreshTokenAsync(command.RefreshToken);
        if (result.IsError)
        {
            _logger.LogWarning("Logout failed for refresh token: {RefreshToken}", command.RefreshToken);
            return userResult.Errors;
        }

        var user = userResult.Value;
        await _eventPublisher.PublishAsync(
                    new UserLoggedOutEvent(user!.UserId, user.Username, DateTime.UtcNow),
                    cancellationToken);

        _logger.LogInformation("Logout successful for refresh token: {RefreshToken}", command.RefreshToken);

        return result;
    }
}