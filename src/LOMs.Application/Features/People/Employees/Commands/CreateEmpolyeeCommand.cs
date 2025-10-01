using LiteBus.Commands.Abstractions;
using LOMs.Application.Features.People.Clients.Commands.CreateClient;
using LOMs.Application.Features.People.Employees.Dtos;
using LOMs.Domain.Common.Results;
using LOMs.Domain.Identity;

namespace LOMs.Application.Features.People.Employees.Commands;

public sealed record CreateEmployeeCommand (Role Role,string Email,CreatePersonCommand Person) : ICommand<Result<EmployeeDto>>;