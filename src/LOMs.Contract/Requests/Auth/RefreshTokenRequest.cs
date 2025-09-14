namespace LOMs.Contract.Requests.Auth;

public class RefreshTokenRequest
{
    public string RefreshToken { get; init; } = default!;
}
