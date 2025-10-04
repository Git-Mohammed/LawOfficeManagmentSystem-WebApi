using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.Auth;

public class RefreshTokenRequest
{
    [Required(ErrorMessage = "Refresh token is required")]
    [StringLength(500, ErrorMessage = "Refresh token must not exceed 500 characters")]
    public string RefreshToken { get; init; } = default!;
}
