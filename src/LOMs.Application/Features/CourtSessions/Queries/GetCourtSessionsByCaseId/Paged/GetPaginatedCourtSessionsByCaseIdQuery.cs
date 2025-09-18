using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.CourtSessions.Dtos;
using LOMs.Domain.Common.Results;
using LOMs.Application.Common.Models;

namespace LOMs.Application.Features.CourtSessions.Queries.GetCourtSessionsByCaseId.Paged;

public sealed record GetPaginatedCourtSessionsByCaseIdQuery(
    Guid CaseId,
    int PageNumber = 1,
    int PageSize = 20,
    DateTimeOffset? FromDate = null
) : IQuery<Result<PaginatedList<CourtSessionDto>>>;
