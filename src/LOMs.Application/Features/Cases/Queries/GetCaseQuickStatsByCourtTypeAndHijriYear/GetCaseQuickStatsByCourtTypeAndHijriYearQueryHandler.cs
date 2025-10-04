using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Cases.Dtos;
using LOMs.Domain.Cases.CourtTypes;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Cases.Queries.GetCaseQuickStatsByCourtTypeAndHijriYear;

public sealed class GetCaseQuickStatsByCourtTypeAndHijriYearQueryHandler
    : IQueryHandler<GetCaseQuickStatsByCourtTypeAndHijriYearQuery, Result<PaginatedList<CaseQuickStatsDto>>>
{
    private readonly ILogger<GetCaseQuickStatsByCourtTypeAndHijriYearQueryHandler> _logger;
    private readonly IAppDbContext _context;

    public GetCaseQuickStatsByCourtTypeAndHijriYearQueryHandler(
        ILogger<GetCaseQuickStatsByCourtTypeAndHijriYearQueryHandler> logger,
        IAppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<PaginatedList<CaseQuickStatsDto>>> HandleAsync(
        GetCaseQuickStatsByCourtTypeAndHijriYearQuery query,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Fetching case stats for CourtTypeId: {CourtTypeId}, HijriYear: {HijriYear}, Page: {PageNumber}, Size: {PageSize}",
            query.CourtTypeId, query.HijriYear, query.PageNumber, query.PageSize);

        // 1. Get CourtType
        var courtType = await _context.CourtTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(ct => ct.Id == query.CourtTypeId, cancellationToken);

        if (courtType is null)
        {
            _logger.LogWarning("CourtType not found: {CourtTypeId}", query.CourtTypeId);
            return CourtTypeErrors.NotFound;
        }

        // 2. Build base query with joins
        var baseQuery =
  from caseEntity in _context.Cases.AsNoTracking()
  from clientCase in caseEntity.ClientCases
  join fileView in _context.ClientFilesWithFileNumber
      on clientCase.ClientFileId equals fileView.Id
  join employee in _context.Employees.AsNoTracking()
      on caseEntity.CreatedBy equals employee.UserId into employeeGroup
  from employee in employeeGroup.DefaultIfEmpty()
  select new CaseQuickStatsDto
            {
                CaseId = caseEntity.Id,
                CaseNumber = caseEntity.CaseNumber ?? string.Empty,
                FileNumber = fileView.FileNumber,
                ClientName = caseEntity.ClientCases
                    .Select(cc => cc.Client!.Person!.FullName)
                    .FirstOrDefault() ?? "Unknown",
                Status = caseEntity.Status,
                CreatedDate = caseEntity.CreatedAtUtc.ToString("yyyy-MM-dd"),
                EmployeeNameCreatedBy = employee != null ? employee.Person!.FullName : "N/A",
                EmployeeIdCreatedBy =   employee.Id,
                CaseSubject = caseEntity.CaseSubject ?? string.Empty
            };

        // 3. Apply status filter if provided
        if (query.Status is not null)
        {
            baseQuery = baseQuery.Where(x => x.Status == query.Status);
        } else
        {
            baseQuery = baseQuery.Where(x => x.Status != CaseStatus.Draft);
        }

        // 4. Pagination
        var totalCount = await baseQuery.CountAsync(cancellationToken);

        var items = await baseQuery
            .OrderBy(x => x.FileNumber)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        // 5. Return paginated result
        return new PaginatedList<CaseQuickStatsDto>(
            items, query.PageNumber, query.PageSize, totalCount);
    }
}
