namespace LOMs.Application.Features.Auth.Dtos;

public sealed record TokenDto(
    AppUserDto User,
    string AccessToken,
    string? RefreshToken = null,
    DateTime ExpiresOn = default,
    bool SetPermanentPassword = false);
