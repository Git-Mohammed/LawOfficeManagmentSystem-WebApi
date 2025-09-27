using LOMs.Domain.Common.Results;

namespace LOMs.Domain.Identity;

public static class UserErrors
{
    public static Error UserNotFound =>
        Error.NotFound("Identity.UserNotFound", "User not found.");

    public static Error InvalidCredentials =>
        Error.Validation("Identity.InvalidCredentials", "Username or password is incorrect.");

    public static Error EmailNotConfirmed =>
        Error.Validation("Identity.EmailNotConfirmed", "Email address is not confirmed.");

    public static Error TokenGenerationFailed =>
        Error.Failure("Identity.TokenGenerationFailed", "Failed to generate authentication token.");

    public static Error InvalidRefreshToken =>
        Error.Validation("Identity.InvalidRefreshToken", "The refresh token is invalid or expired.");

    public static Error RefreshTokenUpdateFailed =>
        Error.Failure("Identity.RefreshTokenUpdateFailed", "Failed to update refresh token.");

    public static Error RefreshTokenRemoveFailed =>
        Error.Failure("Identity.RefreshTokenRemoveFailed", "Failed to remove refresh token.");

    public static Error PasswordResetFailed =>
        Error.Failure("Identity.PasswordResetFailed", "Password reset failed.");

    public static Error PasswordChangeFailed =>
        Error.Failure("Identity.PasswordChangeFailed", "Password change failed.");

    public static Error InvalidNewPassword =>
        Error.Validation("Identity.InvalidNewPassword", "The new password does not meet security requirements.");

    public static Error UnauthorizedPasswordChange =>
        Error.Validation("Identity.UnauthorizedPasswordChange", "You are not authorized to change this password.");

    public static Error UserCreationFailed =>
        Error.Failure("Identity.UserCreationFailed", "User could not be created.");
}
