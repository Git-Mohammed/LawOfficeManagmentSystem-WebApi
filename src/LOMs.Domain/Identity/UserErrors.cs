using LOMs.Domain.Common.Results;

namespace LOMs.Domain.Identity;

public static class UserErrors
{
    public static Error UserNotFound =>
        Error.NotFound("Auth.UserNotFound", "User not found.");

    public static Error InvalidCredentials =>
        Error.Validation("Auth.InvalidCredentials", "Username or password is incorrect.");

    public static Error EmailNotConfirmed =>
        Error.Validation("Auth.EmailNotConfirmed", "Email not confirmed.");

    public static Error TokenGenerationFailed =>
        Error.Failure("Auth.TokenGenerationFailed", "Failed to generate token.");

    public static Error InvalidRefreshToken =>
        Error.Validation("Auth.InvalidRefreshToken", "Invalid or expired refresh token.");

    public static Error RefreshTokenUpdateFailed =>
        Error.Failure("Auth.RefreshTokenUpdateFailed", "Failed to update refresh token.");

    public static Error RefreshTokenRemoveFailed =>
        Error.Failure("Auth.RefreshTokenRemoveFailed", "Failed to remove refresh token.");

    public static Error PasswordResetFailed =>
        Error.Failure("Auth.ResetFailed", "Password reset failed.");

    public static Error UserCreationFailed =>
        Error.Failure("Identity_User_not_created", "User could not be created.");
}
