using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.Clients;

public class CreateClientRequest
{
    [Required]
    public PersonRequest Person { get; set; } = new();
}

public class PersonRequest
{
    [Required(ErrorMessage = "Full name is required.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "National ID is required.")]
    public string NationalId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Birth date is required.")]
    public DateOnly BirthDate { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^\+?\d{7,15}$", ErrorMessage = "Phone number must be 7–15 digits and may start with '+'.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address is required.")]
    public string Address { get; set; } = string.Empty;
}
