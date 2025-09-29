using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.Clients
{
    public sealed class CreatePhoneRequest : IValidatableObject
    {
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\+?\d{7,15}$", ErrorMessage = "Phone number must consist of 7 to 15 digits and may start with '+'")]
        public string Number { get; set; } = null!;

        [Required(ErrorMessage = "Phone code is required")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Phone code must be between 1 and 10 characters")]
        public string Code { get; set; } = null!;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Number != null && Code != null && Number.StartsWith(Code))
            {
                yield return new ValidationResult(
                    "Phone number must not start with the phone code.",
                    new[] { nameof(Number), nameof(Code) });
            }
        }
    }
}
