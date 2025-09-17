using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.CourtSessions.Dtos;
using LOMs.Domain.Cases.CourtSessions;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;

namespace LOMs.Application.Features.CourtSessions.Queries.GetFutureCourtSessions;

public sealed class GetFutureCourtSessionsQueryHandler(
    IAppDbContext context
) : IQueryHandler<GetFutureCourtSessionsQuery, Result<IReadOnlyList<CourtSessionDto>>>
{
    private readonly IAppDbContext _context = context;

    public async Task<Result<IReadOnlyList<CourtSessionDto>>> HandleAsync(GetFutureCourtSessionsQuery query, CancellationToken cancellationToken = default)
    {
        var sessions = await _context.CourtSessions
            .Where(cs => cs.ScheduledAt > query.ReferenceTime)
            .OrderBy(cs => cs.ScheduledAt)
            .Select(cs => new CourtSessionDto
            {
                CourtSessionId = cs.Id,
                CaseId = cs.CaseId,
                ScheduledAt = cs.ScheduledAt,
                AssignedTasks = cs.AssignedTasks,
                AssignedEmployeeId = cs.AssignedEmployeeId,
                AssignedEmployeeName = cs.AssignedEmployee!.Person.FullName ?? string.Empty
            })
            .ToListAsync(cancellationToken);

        return sessions;
    }
}
