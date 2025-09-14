using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.Auth;

public class RefreshTokenRequest
{
    [Required(ErrorMessage = "رمز التحديث مطلوب")]
    [StringLength(500, ErrorMessage = "رمز التحديث يجب ألا يتجاوز 500 حرف")]
    public string RefreshToken { get; init; } = default!;
}
