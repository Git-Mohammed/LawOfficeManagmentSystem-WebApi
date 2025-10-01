using LOMs.Application.Features.People.Clients.Commands.CreateClient;
using LOMs.Application.Features.People.Clients.Dtos;

namespace LOMs.Application.Features.People.Employees.Dtos;

public sealed class EmployeeDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public DateOnly BirthDate { get; init; }
    public string PhoneNumber { get; init; } = string.Empty;
    public string NatId { get; init; } = string.Empty;
    public string CountryCode { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public string Address {get; init; } = string.Empty;
}