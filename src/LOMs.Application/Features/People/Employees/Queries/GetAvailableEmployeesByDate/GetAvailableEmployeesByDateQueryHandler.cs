using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.People.Employees.Dtos;
using LOMs.Application.Features.People.Employees.Queries.GetAvailableEmployeesByDate;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Employees.Queries;

public sealed class GetAvailableEmployeesByDateQueryHandler(
    IAppDbContext context,
    ILogger<GetAvailableEmployeesByDateQueryHandler> logger)
    : IQueryHandler<GetAvailableEmployeesByDateQuery, Result<IReadOnlyList<EmployeeNameDto>>>
{
    public async Task<Result<IReadOnlyList<EmployeeNameDto>>> HandleAsync(
        GetAvailableEmployeesByDateQuery query,
        CancellationToken ct)
    {
        if (query.TargetDate == default)
        {
            logger.LogWarning("Invalid date provided for employee availability check.");
            return Error.Validation("Date.Invalid", "Target date must be specified.");
        }

        var busyEmployeeIds = await context.CourtSessions
            .Where(cs => cs.ScheduledAt == query.TargetDate)
            .Select(cs => cs.AssignedEmployeeId)
            .Distinct()
            .ToListAsync(ct);

        var availableEmployees = await context.Employees
            .Include(e => e.Person)
            .AsNoTracking()
            .Where(e => !busyEmployeeIds.Contains(e.Id))
            .Select(e => new EmployeeNameDto
            {
                EmployeeId = e.Id,
                FullName = e!.Person!.FullName
            })
            .ToListAsync(ct);

        return availableEmployees;
    }
}
