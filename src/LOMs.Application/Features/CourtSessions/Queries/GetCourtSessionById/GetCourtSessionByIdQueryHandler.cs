using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.CourtSessions.Dtos;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.CourtSessions.Queries.GetCourtSessionById;

public sealed class GetCourtSessionByIdQueryHandler(
    IAppDbContext context,
    ILogger<GetCourtSessionByIdQueryHandler> logger
) : IQueryHandler<GetCourtSessionByIdQuery, Result<CourtSessionDto>>
{
    private readonly IAppDbContext _context = context;
    private readonly ILogger<GetCourtSessionByIdQueryHandler> _logger = logger;

    public async Task<Result<CourtSessionDto>> HandleAsync(GetCourtSessionByIdQuery query, CancellationToken cancellationToken = default)
    {
        var session = await _context.CourtSessions
            .Include(cs => cs.AssignedEmployee)
                .ThenInclude(e => e.Person)
            .Where(cs => cs.CaseId == query.CaseId && cs.Id == query.SessionId)
            .Select(cs => new CourtSessionDto
            {
                CourtSessionId = cs.Id,
                CaseId = cs.CaseId,
                AssignedEmployeeId = cs.AssignedEmployeeId,
                AssignedEmployeeName = cs.AssignedEmployee != null ? cs.AssignedEmployee.Person.FullName : "Unknown",
                ScheduledAt = cs.ScheduledAt,
                AssignedTasks = cs.AssignedTasks,
                IsSessionDateExpired = cs.IsSessionDateExpired
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (session is null)
        {
            _logger.LogWarning("CourtSession not found. CaseId: {CaseId}, SessionId: {SessionId}", query.CaseId, query.SessionId);
            return Error.NotFound("CourtSession not found.");
        }

        return session;
    }
}
