using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.CourtSessions.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.CourtSessions.Queries.GetFutureCourtSessions.Paged;
public sealed record GetPaginatedFutureCourtSessionsQuery(
    int PageNumber = 1,
    int PageSize = 20
) : IQuery<Result<PaginatedList<GetFutureCourtSessionDto>>>;
