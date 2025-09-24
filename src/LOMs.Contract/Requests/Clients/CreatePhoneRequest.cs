using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.Clients
{
    public sealed class CreatePhoneRequest : IValidatableObject
    {
        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression(@"^\+?\d{7,15}$", ErrorMessage = "رقم الهاتف يجب أن يتكون من 7 إلى 15 رقمًا وقد يبدأ بـ '+'")]
        public string Number { get; set; } = null!;

        [Required(ErrorMessage = "رمز الهاتف مطلوب")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "رمز الهاتف يجب أن يكون بين 1 و 10 أحرف")]
        public string Code { get; set; } = null!;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Number != null && Code != null && Number.StartsWith(Code))
            {
                yield return new ValidationResult(
                    "رقم الهاتف يجب ألا يبدأ برمز الهاتف.",
                    new[] { nameof(Number), nameof(Code) });
            }
        }
    }
}
