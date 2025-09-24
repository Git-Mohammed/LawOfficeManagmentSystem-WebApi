using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.Clients;

public class CreatePersonRequest
{
    [Required(ErrorMessage = "الاسم الكامل مطلوب.")]
    [MinLength(3, ErrorMessage = "الاسم الكامل يجب أن لا يقل عن 3 أحرف.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "رقم الهوية الوطنية مطلوب.")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "رقم الهوية الوطنية يجب أن يتكون من 10 أرقام.")]
    public string NationalId { get; set; } = string.Empty;

    [Required(ErrorMessage = "تاريخ الميلاد مطلوب.")]
    public DateOnly BirthDate { get; set; }

    [Required(ErrorMessage = "رقم الهاتف مطلوب.")]
    public CreatePhoneRequest Phone { get; set; } = new CreatePhoneRequest();

    [MinLength(10, ErrorMessage = "العنوان يجب أن يحتوي على تفاصيل كافية ولا يقل عن 10 أحرف.")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "CountryCode is required")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "CountryCode must be exactly 2 characters")]
    [RegularExpression("^[A-Z]{2}$", ErrorMessage = "CountryCode must be a valid ISO Alpha-2 code (e.g., EG, US, FR)")]
    public string CountryCode { get; set; } = string.Empty;
}
