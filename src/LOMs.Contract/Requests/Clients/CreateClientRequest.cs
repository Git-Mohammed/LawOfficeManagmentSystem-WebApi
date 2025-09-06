using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.Clients;

public class CreateClientRequest
{
    [Required]
    public PersonRequest Person { get; set; } = new();
}
