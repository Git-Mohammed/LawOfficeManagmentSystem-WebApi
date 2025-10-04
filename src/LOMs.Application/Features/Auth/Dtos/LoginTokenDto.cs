namespace LOMs.Application.Features.Auth.Dtos;

public sealed record LoginTokenDto(
    AppUserDto User,
    TokenResponse TokenResponse);
