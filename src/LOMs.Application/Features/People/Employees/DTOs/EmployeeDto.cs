using LOMs.Application.Features.People.Clients.Commands.CreateClient;
using LOMs.Application.Features.People.Clients.Dtos;

namespace LOMs.Application.Features.People.Employees.DTOs;

public sealed class EmployeeDto
{
    public Guid Id { get; init; }
    public string Role { get; init; }
    public string Email { get; init; }
    public PersonDto Person { get; init; }
}