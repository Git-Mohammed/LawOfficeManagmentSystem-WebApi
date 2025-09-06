using System.ComponentModel.DataAnnotations;
using LOMs.Contract.Requests.Clients;

namespace LOMs.Contract.Requests.Employees;

public class CreateEmployeeRequest
{
    [Required] public PersonRequest Person { get; set; } = null!;
    [Required(ErrorMessage = "Role is required")]
    public string Role { get; set; } = null!;
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email is invalid.")]
    public string Email { get; set; } = null!;

}