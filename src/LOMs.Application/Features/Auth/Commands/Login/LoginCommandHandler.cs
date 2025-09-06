using LiteBus.Commands.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Auth.Dtos;
using LOMs.Domain.Common.Results;
using LOMs.Domain.Identity.DomainEvents;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace LOMs.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler(ILogger<LoginCommandHandler> logger,
         IIdentityService identityService, IPasswordGenerator passwordGenerator, IDomainEventPublisher eventPublisher)
    : ICommandHandler<LoginCommand, Result<TokenDto>>
    {
        private readonly ILogger<LoginCommandHandler> _logger = logger;
        private readonly IIdentityService _identityService = identityService;
        private readonly IPasswordGenerator _passwordGenerator = passwordGenerator;
        private readonly IDomainEventPublisher _eventPublisher = eventPublisher;

        public async Task<Result<TokenDto>> HandleAsync(LoginCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Login attempt for user: {Username}", command.Username);

            var username = command.Username.Trim().ToLower();
            var userResult = await _identityService.GetUserByNameAsync(username);

            if (userResult.IsError || userResult.Value is null)
            {
                _logger.LogWarning("Login failed: user not found. Username: {Username}", username);
                return userResult.Errors;
            }

            var user = userResult.Value;
            //var decryptedPassword = _passwordGenerator.IsTempPassword(command.Password);
            var decryptedPassword = _passwordGenerator.DecryptPassword(command.Password);
            var isPermanentPassword = decryptedPassword.StartsWith("Temp-");
            var password = decryptedPassword;

            var loginResult = await _identityService.AuthenticateAsync(username, command.Password!);

            if (loginResult.IsError)
            {
                _logger.LogWarning("Login failed: authentication error for user {Username}", username);
                return loginResult.Errors;
            }

            var tokenResult = await _identityService.GenerateTokenAsync(user);
            if (tokenResult.IsError)
            {
                _logger.LogError("Login failed: token generation error for user {Username}", username);
                return tokenResult.Errors;
            }

            var addRTResult = await _identityService.UpdateRefreshToken(user.UserId, tokenResult.Value.RefreshToken!);
            if (addRTResult.IsError)
            {
                _logger.LogError("Login failed: could not update refresh token for user {UserId}", user.UserId);
                return addRTResult.Errors;
            }

            var tokenDto = new TokenDto(
                User: user,
                AccessToken: tokenResult.Value.AccessToken,
                RefreshToken: tokenResult.Value.RefreshToken,
                ExpiresOn: tokenResult.Value.ExpiresOn,
                SetPermanentPassword: isPermanentPassword
            );

         
            await _eventPublisher.PublishAsync(new UserLoggedInEvent(user.UserId,user.Username,DateTime.UtcNow), cancellationToken);

            _logger.LogInformation("User logged in successfully. Id: {UserId}", user.UserId);
            return tokenDto;
        }
    }
}