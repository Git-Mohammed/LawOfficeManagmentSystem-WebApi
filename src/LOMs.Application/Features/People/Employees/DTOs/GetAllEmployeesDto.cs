using LOMs.Domain.Identity;

namespace LOMs.Application.Features.People.Employees.Dtos;

public sealed class GetAllEmployeesDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public DateOnly BirthDate { get; init; }
    public string PhoneNumber { get; init; } = string.Empty;
    public string NatId { get; init; } = string.Empty;
    public string CountryCode { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public Role Role { get; init; }
    public string Address {get; init; } = string.Empty;

    public bool IsActive { get; init; }
}
