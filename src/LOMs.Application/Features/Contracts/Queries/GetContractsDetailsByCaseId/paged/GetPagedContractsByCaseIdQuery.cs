using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Contracts.Queries.GetContractsDetailsByCaseId.Paged;
public sealed class GetPagedContractsByCaseIdQuery(
    Guid caseId,
    int pageNumber,
    int pageSize)
    : IQuery<Result<PaginatedList<ContractDetailsDto>>>
{
    public Guid CaseId { get; } = caseId;
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;
}
