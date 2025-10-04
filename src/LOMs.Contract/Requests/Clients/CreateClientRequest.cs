using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.Clients;

public class CreateClientRequest
{
    [Required]
    public CreatePersonRequest Person { get; set; } = new();
}
