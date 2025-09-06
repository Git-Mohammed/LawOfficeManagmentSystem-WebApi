namespace LOMs.Application.Features.Auth.Dtos
{
    public class TokenDto
    {
        public UserDto User { get; init; } = new();
        public string Token { get; init; } = null!;
        public string RefreshToken { get; init; } = null!;
    }

}
