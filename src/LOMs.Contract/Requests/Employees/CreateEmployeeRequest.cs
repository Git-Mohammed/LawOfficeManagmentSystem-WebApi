using System.ComponentModel.DataAnnotations;
using LOMs.Contract.Commons.Enums;
using LOMs.Contract.Requests.Clients;

namespace LOMs.Contract.Requests.Employees;

public class CreateEmployeeRequest
{
    [Required] 
    public CreatePersonRequest Person { get; set; } = null!;

    [Required(ErrorMessage = "Role is required")]
    public Role Role { get; set; } 
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email is invalid.")]
    public string Email { get; set; } = null!;

}