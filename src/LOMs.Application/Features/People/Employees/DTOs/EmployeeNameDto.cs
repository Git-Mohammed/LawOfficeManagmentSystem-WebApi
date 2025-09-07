using LOMs.Application.Features.People.Clients.Dtos;

namespace LOMs.Application.Features.People.Employees.DTOs;

public sealed class EmployeeNameDto
{
    public Guid EmployeeId { get; init; }
    public string FullName { get; init; } = string.Empty;
}