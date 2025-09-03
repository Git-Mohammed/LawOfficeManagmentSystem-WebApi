using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.People.Employees.DTOs;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.People.Employees.Queries;

public sealed record GetEmployeeByIdQuery(Guid Id):IQuery<Result<EmployeeDto>>;