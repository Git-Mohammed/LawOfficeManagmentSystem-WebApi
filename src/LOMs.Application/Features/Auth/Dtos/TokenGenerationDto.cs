namespace LOMs.Application.Features.Auth.Dtos;

public sealed record TokenGenerationDto(
    string AccessToken,
    string? RefreshToken = null,
    DateTime ExpiresOn = default);
