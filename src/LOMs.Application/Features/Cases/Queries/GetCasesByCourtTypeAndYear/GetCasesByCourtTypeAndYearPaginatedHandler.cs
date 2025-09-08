using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Cases.Dtos;
using LOMs.Domain.Cases;
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

        var baseQuery = _context.Cases
            .AsNoTracking()
            .Include(c => c.ClientCases)
                .ThenInclude(cc => cc.ClientFile)
            .Include(c => c.ClientCases)
                .ThenInclude(cc => cc.Client)
                    .ThenInclude(cl => cl.Person)
            .Where(c => c.CourtTypeId == query.CourtTypeId &&
                        c.ClientCases.Any(cc => cc.ClientFile.HijriYear == query.HijriYear))
            .OrderBy(c => c.CaseNumber);

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        var items = await baseQuery
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        var dtoList = _mapper.Map<List<Case>, List<CaseQuickStatsDto>>(items);

        var paginatedResult = new PaginatedList<CaseQuickStatsDto>
        {
            Items = dtoList,
            TotalItemsCount = totalCount,
            PageIndex = query.PageNumber,
            PageSize = query.PageSize
        };

        return paginatedResult;
    }
}
