using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Cases.Dtos;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Cases.Queries.GetCaseQuickStatsByCourtTypeAndHijriYear;

public sealed record GetCaseQuickStatsByCourtTypeAndHijriYearQuery(
    Guid CourtTypeId,
    short HijriYear,
    int PageNumber,
    int PageSize,
    CaseStatus? Status = null
) : IQuery<Result<PaginatedList<CaseQuickStatsDto>>>;
