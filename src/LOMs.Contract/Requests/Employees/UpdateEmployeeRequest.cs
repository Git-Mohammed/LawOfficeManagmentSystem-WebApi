using System.ComponentModel.DataAnnotations;
using LOMs.Contract.Commons.Enums;
using LOMs.Contract.Requests.Clients;

namespace LOMs.Contract.Requests.Employees;

public sealed class UpdateEmployeeRequest
{

    [Required(ErrorMessage = "Full name is required.")]
    [MinLength(3, ErrorMessage = "Full name must be at least 3 characters long.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "National ID is required.")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "National ID must be exactly 10 digits.")]
    public string NationalId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Birth date is required.")]
    public DateOnly BirthDate { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    public CreatePhoneRequest Phone { get; set; } = null!;

    [MinLength(10, ErrorMessage = "Address must be at least 10 characters long.")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "Country code is required.")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "Country code must be exactly 2 characters.")]
    [RegularExpression("^[A-Z]{2}$", ErrorMessage = "Country code must be a valid ISO Alpha-2 code (e.g., EG, US, FR).")]
    public string CountryCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role is required.")]
    public Role Role { get; set; } 

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email format is invalid.")]
    public string Email { get; set; } = null!;

    public bool IsActive { get; set; }
}