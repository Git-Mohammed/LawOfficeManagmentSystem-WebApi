using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Contracts.Queries.GetContractList.Paged;

  public sealed record GetPagedContractListQuery
(
    int pageIndex = 1,
    int pageSize = 10)
    : IQuery<Result<PaginatedList<ContractListItemDto>>>
    {
        public int PageIndex { get; } = pageIndex;
        public int PageSize { get; } = pageSize;
    }