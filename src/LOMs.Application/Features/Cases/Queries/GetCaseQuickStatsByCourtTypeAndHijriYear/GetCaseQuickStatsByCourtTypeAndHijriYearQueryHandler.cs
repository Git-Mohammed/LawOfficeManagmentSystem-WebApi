using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Cases.Dtos;
using LOMs.Domain.Cases;
using LOMs.Domain.Cases.ClientFiles;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Cases.Queries.GetCaseQuickStatsByCourtTypeAndHijriYear;

public class GetCaseQuickStatsByCourtTypeAndHijriYearQueryHandler
    : IQueryHandler<GetCaseQuickStatsByCourtTypeAndHijriYearQuery, Result<PaginatedList<CaseQuickStatsDto>>>
{
    private readonly ILogger<GetCaseQuickStatsByCourtTypeAndHijriYearQueryHandler> _logger;
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetCaseQuickStatsByCourtTypeAndHijriYearQueryHandler(
        ILogger<GetCaseQuickStatsByCourtTypeAndHijriYearQueryHandler> logger,
        IAppDbContext context,
        IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedList<CaseQuickStatsDto>>> HandleAsync(
        GetCaseQuickStatsByCourtTypeAndHijriYearQuery query,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Fetching cases for CourtTypeId: {CourtTypeId}, HijriYear: {HijriYear}, Page: {PageNumber}, Size: {PageSize}",
            query.CourtTypeId, query.HijriYear, query.PageNumber, query.PageSize);

        var baseQuery = from caseEntity in _context.Cases.AsNoTracking()
                        from clientCase in caseEntity.ClientCases
                        join fileView in _context.ClientFilesWithFileNumber
                            on clientCase.ClientFileId equals fileView.Id
                        where caseEntity.CourtTypeId == query.CourtTypeId
                              && fileView.HijriYear == query.HijriYear
                        select new CaseQuickStatsDto
                        {
                            CaseId = caseEntity.Id,
                            CaseNumber = caseEntity.CaseNumber,
                            FileNumber = fileView.FileNumber,
                            ClientName = clientCase.Client!.Person!.FullName,
                            Status = caseEntity.Status
                        };

        // Apply status filter if provided
        if (query.Status is not null)
        {
            baseQuery = baseQuery.Where(x => x.Status == query.Status);
        }

        // Step 2: Paginate efficiently
        var totalCount = await baseQuery.CountAsync(cancellationToken);

        var items = await baseQuery
            .OrderBy(dto => dto.FileNumber) // Optional: order by FileNumber
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        var paginatedResult = new PaginatedList<CaseQuickStatsDto>(
            items, query.PageNumber, query.PageSize, totalCount);

        return paginatedResult;
    }

}
