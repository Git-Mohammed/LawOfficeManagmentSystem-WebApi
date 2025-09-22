using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Cases.Dtos;
using LOMs.Domain.Cases;
using LOMs.Domain.Cases.ClientFiles;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Cases.Queries.GetCasesByCourtTypeAndYear;

public class GetCasesByCourtTypeAndYearPaginatedHandler
    : IQueryHandler<GetCasesByCourtTypeAndYearPaginatedQuery, Result<PaginatedList<CaseQuickStatsDto>>>
{
    private readonly ILogger<GetCasesByCourtTypeAndYearPaginatedHandler> _logger;
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetCasesByCourtTypeAndYearPaginatedHandler(
        ILogger<GetCasesByCourtTypeAndYearPaginatedHandler> logger,
        IAppDbContext context,
        IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedList<CaseQuickStatsDto>>> HandleAsync(
        GetCasesByCourtTypeAndYearPaginatedQuery query,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Fetching cases for CourtTypeId: {CourtTypeId}, HijriYear: {HijriYear}, Page: {PageNumber}, Size: {PageSize}",
            query.CourtTypeId, query.HijriYear, query.PageNumber, query.PageSize);

        // Step 1: Join Cases with ClientFiles view
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
