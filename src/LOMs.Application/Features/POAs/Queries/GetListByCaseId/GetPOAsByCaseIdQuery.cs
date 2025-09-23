using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.POAs.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.POAs.Queries.GetListByCaseId;
public sealed record GetPOAsByCaseIdQuery(
    Guid CaseId,
    int PageNumber,
    int PageSize)
    : IQuery<Result<PaginatedList<POAListRowDto>>>;

