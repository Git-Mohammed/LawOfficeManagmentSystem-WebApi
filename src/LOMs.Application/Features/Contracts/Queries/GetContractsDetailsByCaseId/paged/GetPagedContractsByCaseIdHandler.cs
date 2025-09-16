using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Application.Features.CourtSessions.Dtos;
using LOMs.Application.Features.CourtSessions.Queries.GetCourtSessionsByCaseId.Paged;
using LOMs.Domain.Cases;
using LOMs.Domain.Cases.Contracts;
using LOMs.Domain.Cases.CourtSessions;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Contracts.Queries.GetContractsDetailsByCaseId.Paged;

public sealed class GetPaginatedCourtSessionsByCaseIdQueryHandler(
    IAppDbContext context,
    ILogger<GetPaginatedCourtSessionsByCaseIdQueryHandler> logger)
    : IQueryHandler<GetPaginatedCourtSessionsByCaseIdQuery, Result<PaginatedList<CourtSessionDto>>>
{
    public async Task<Result<PaginatedList<CourtSessionDto>>> HandleAsync(
        GetPaginatedCourtSessionsByCaseIdQuery query,
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

        var caseExists = await context.Cases
            .AsNoTracking()
            .AnyAsync(c => c.Id == query.CaseId, ct);

        if (!caseExists)
        {
            logger.LogWarning("Fetching court sessions failed: Case not found. CaseId: {CaseId}", query.CaseId);
            return CourtSessionErrors.CaseNotFound;
        }

        var sessionsQuery = context.CourtSessions
            .AsNoTracking()
            .Where(cs => cs.CaseId == query.CaseId);

        if (query.FromDate is not null)
        {
            sessionsQuery = sessionsQuery.Where(cs => cs.ScheduledAt >= query.FromDate);
        }

        var totalCount = await sessionsQuery.CountAsync(ct);

        var sessions = await sessionsQuery
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



