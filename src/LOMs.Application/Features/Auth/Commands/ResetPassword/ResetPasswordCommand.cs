using LiteBus.Commands.Abstractions;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Auth.Commands.ResetPassword;

public sealed record ResetPasswordCommand(
    string Username,
    string Token,
    string NewPassword
) : ICommand<Result<Success>>;