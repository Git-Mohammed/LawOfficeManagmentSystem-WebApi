using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.People.Employees.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.People.Employees.Queries.GetAvailableEmployeesByDate;

public sealed record GetAvailableEmployeesByDateQuery(DateTimeOffset TargetDate)
    : IQuery<Result<IReadOnlyList<EmployeeNameDto>>>;
