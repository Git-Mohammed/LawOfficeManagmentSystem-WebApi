using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.CourtSessions.Dtos;
using LOMs.Domain.Cases.CourtSessions;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.CourtSessions.Queries.GetFutureCourtSessions.Paged;

public sealed class GetPaginatedFutureCourtSessionsQueryHandler(
    IAppDbContext context,
    ILogger<GetPaginatedFutureCourtSessionsQueryHandler> logger)
    : IQueryHandler<GetPaginatedFutureCourtSessionsQuery, Result<PaginatedList<GetFutureCourtSessionDto>>>
{
    public async Task<Result<PaginatedList<GetFutureCourtSessionDto>>> HandleAsync(
        GetPaginatedFutureCourtSessionsQuery query,
        CancellationToken ct)
    {
        if (query.PageNumber < 1)
        {
            logger.LogWarning("Invalid page number: {PageNumber}", query.PageNumber);
            return Error.Validation("Pagination.InvalidPageNumber", "Page number must be at least 1.");
        }

        if (query.PageSize < 1 || query.PageSize > 100)
        {
            logger.LogWarning("Invalid page size: {PageSize}", query.PageSize);
            return Error.Validation("Pagination.InvalidPageSize", "Page size must be between 1 and 100.");
        }

        var now = DateTimeOffset.UtcNow;

        var totalCount = await context.CourtSessions
            .Where(cs => cs.ScheduledAt > now)
            .CountAsync(ct);

        var sessions = await context.CourtSessions
            .AsNoTracking()
            .Where(cs => cs.ScheduledAt > now)
            .OrderBy(cs => cs.ScheduledAt)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
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
            .ToListAsync(ct);

        var paginated = new PaginatedList<GetFutureCourtSessionDto>(
            items: sessions,
            pageIndex: query.PageNumber,
            pageSize: query.PageSize,
            totalItemsCount: totalCount
        );

        return paginated;
    }
}
