namespace LOMs.Application.Features.Auth.Dtos;

public sealed record TokenResponse(
    string AccessToken,
    string RefreshToken,
    DateTimeOffset AccessTokenExpiresOn = default,
    DateTimeOffset RefreshTokenExpiresOn = default);
