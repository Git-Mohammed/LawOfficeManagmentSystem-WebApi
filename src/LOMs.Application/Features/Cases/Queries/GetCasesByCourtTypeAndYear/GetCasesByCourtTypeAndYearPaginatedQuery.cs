using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Cases.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Cases.Queries.GetCasesByCourtTypeAndYear;

public sealed record GetCasesByCourtTypeAndYearPaginatedQuery(
    Guid CourtTypeId,
    short HijriYear,
    int PageNumber,
    int PageSize
) : IQuery<Result<PaginatedList<CaseQuickStatsDto>>>;

