using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.People.Employees.DTOs;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.People.Employees.Queries.GetEmployees;

public record GetEmployeesQuery():IQuery<Result<List<EmployeeShortDto>>>;