using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.Clients;

public class CreatePersonRequest
{
    [Required(ErrorMessage = "Full name is required.")]
    [MinLength(3, ErrorMessage = "Full name must be at least 3 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "National ID is required.")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "National ID must consist of 10 digits.")]
    public string NationalId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Birth date is required.")]
    public DateOnly BirthDate { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    public CreatePhoneRequest Phone { get; set; } = new CreatePhoneRequest();

    [MinLength(10, ErrorMessage = "Address must contain sufficient details and be at least 10 characters.")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "CountryCode is required")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "CountryCode must be exactly 2 characters")]
    [RegularExpression("^[A-Z]{2}$", ErrorMessage = "CountryCode must be a valid ISO Alpha-2 code (e.g., EG, US, FR)")]
    public string CountryCode { get; set; } = string.Empty;
}
