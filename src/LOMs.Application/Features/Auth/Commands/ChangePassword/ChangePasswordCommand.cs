using LiteBus.Commands.Abstractions;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Auth.Commands.ResetPassword;

public sealed record ChangePasswordCommand(
    string Username,
    string CurrentPassword,
    string NewPassword
) : ICommand<Result<Success>>;