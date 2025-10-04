using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Contracts.Queries.GetContractsDetailsByCaseId.Paged;
public sealed record GetPagedContractsDetailsByCaseIdQuery(
    Guid CaseId,
    int PageNumber,
    int PageSize)
    : IQuery<Result<PaginatedList<ContractDetailsDto>>>;
