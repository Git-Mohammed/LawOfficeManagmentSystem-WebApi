using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.CourtSessions.Dtos;
using LOMs.Domain.Cases.CourtSessions;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;

namespace LOMs.Application.Features.CourtSessions.Queries.GetFutureCourtSessions;

public sealed class GetFutureCourtSessionsQueryHandler(
    IAppDbContext context
) : IQueryHandler<GetFutureCourtSessionsQuery, Result<IReadOnlyList<GetFutureCourtSessionDto>>>
{
    private readonly IAppDbContext _context = context;

    public async Task<Result<IReadOnlyList<GetFutureCourtSessionDto>>> HandleAsync(GetFutureCourtSessionsQuery query, CancellationToken cancellationToken = default)
    {
        var sessions = await _context.CourtSessions
            .Where(cs => cs.ScheduledAt > query.ReferenceTime)
            .OrderBy(cs => cs.ScheduledAt)
                      .Select(c => new
                      {
                          CourtSession = c,
                          CreatedByEmpoyeeId = context.Employees
                              .Where(e => e.UserId == c.CreatedBy)
                              .Select(e => e.Id)
                              .FirstOrDefault(),
                          CreatedByEmployeeName = context.Employees
                    .Where(e => e.UserId == c.CreatedBy)
                    .Select(e => e.Person.FullName)
                    .FirstOrDefault()
                      })
            .Select(cs => new GetFutureCourtSessionDto
            {
                CourtSessionId = cs.CourtSession.Id,
                CaseId = cs.CourtSession.CaseId,
                ScheduledAt = cs.CourtSession.ScheduledAt,
                CreatedByEmployeeId = cs.CreatedByEmpoyeeId,
                CreatedByEmployeeName = cs.CreatedByEmployeeName ?? string.Empty,
                AssignedEmployeeId = cs.CourtSession.AssignedEmployeeId,
                AssignedEmployeeName = cs.CourtSession.AssignedEmployee!.Person.FullName ?? string.Empty,
                CreatedDate = cs.CourtSession.CreatedAtUtc
            })
            .ToListAsync(cancellationToken);


        return sessions;
    }
}
