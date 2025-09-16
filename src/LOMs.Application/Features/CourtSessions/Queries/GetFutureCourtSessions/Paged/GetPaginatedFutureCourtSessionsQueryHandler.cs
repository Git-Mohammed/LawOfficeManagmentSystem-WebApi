using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.CourtSessions.Dtos;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.CourtSessions.Queries.GetFutureCourtSessions.Paged;

public sealed class GetPaginatedFutureCourtSessionsQueryHandler(
    IAppDbContext context,
    ILogger<GetPaginatedFutureCourtSessionsQueryHandler> logger)
    : IQueryHandler<GetPaginatedFutureCourtSessionsQuery, Result<PaginatedList<CourtSessionDto>>>
{
    public async Task<Result<PaginatedList<CourtSessionDto>>> HandleAsync(
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
            .Select(cs => new CourtSessionDto
            {
                CourtSessionId = cs.Id,
                CaseId = cs.CaseId,
                ScheduledAt = cs.ScheduledAt,
                AssignedTasks = cs.AssignedTasks,
                AssignedEmployeeId = cs.AssignedEmployeeId
            })
            .ToListAsync(ct);

        var paginated = new PaginatedList<CourtSessionDto>(
            items: sessions,
            pageIndex: query.PageNumber,
            pageSize: query.PageSize,
            totalItemsCount: totalCount
        );

        return paginated;
    }
}
