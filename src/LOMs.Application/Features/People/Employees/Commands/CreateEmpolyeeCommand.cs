using LiteBus.Commands.Abstractions;
using LOMs.Application.Features.People.Clients.Commands.CreateClient;
using LOMs.Application.Features.People.Employees.DTOs;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.People.Employees.Commands;

public sealed record CreateEmployeeCommand (string Role,string Email,CreatePersonCommand Person) : ICommand<Result<EmployeeDto>>;