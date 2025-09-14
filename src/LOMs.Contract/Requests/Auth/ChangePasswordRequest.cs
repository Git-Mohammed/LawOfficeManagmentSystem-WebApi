using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.Auth;

public sealed class ChangePasswordRequest
{
    [Required(ErrorMessage = "اسم المستخدم مطلوب")]
    [StringLength(10, ErrorMessage = "اسم المستخدم يجب ألا يتجاوز 100 حرف")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "كلمة المرور الحالية مطلوبة")]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = null!;

    [Required(ErrorMessage = "كلمة المرور الجديدة مطلوبة")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون بين 6 و 100 حرف")]
    public string NewPassword { get; set; } = null!;
}
