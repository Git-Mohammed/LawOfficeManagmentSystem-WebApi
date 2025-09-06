using LiteBus.Commands.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Domain.Common.Results;
using LOMs.Domain.Identity.DomainEvents;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, Result<Success>>
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<ResetPasswordCommandHandler> _logger;
        private readonly IDomainEventPublisher _eventPublisher;

        public ResetPasswordCommandHandler(IIdentityService identityService, ILogger<ResetPasswordCommandHandler> logger
            , IDomainEventPublisher eventPublisher)
        {
            _identityService = identityService;
            _logger = logger;
            _eventPublisher = eventPublisher;
        }

        public async Task<Result<Success>> HandleAsync(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Reset password attempt for user: {Username}", command.Username);
            var userResult = await _identityService.GetUserByNameAsync(command.Username);
            if (userResult.IsError || userResult.Value is null)
            {
                _logger.LogWarning("Password reset failed: user not found. Username: {Username}", command.Username);
                return userResult.Errors;
            }


            var result = await _identityService.ResetPasswordAsync(command.Username, command.Token, command.NewPassword);
            if (result.IsSuccess)
                _logger.LogInformation("Password reset successful for user: {Username}", command.Username);
            else
                _logger.LogWarning("Password reset failed for user: {Username}", command.Username);


            var user = userResult.Value;
            await _eventPublisher.PublishAsync(new UserPasswordResetEvent(user.UserId, user.Username, DateTime.Now), cancellationToken);

            return result;
        }
    }
}