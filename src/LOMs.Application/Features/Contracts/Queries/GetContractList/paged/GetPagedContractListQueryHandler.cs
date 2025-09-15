using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Contracts.Queries.GetContractList.paged;

public sealed class GetPagedContractListQueryHandler(
    IAppDbContext context,
    ILogger<GetPagedContractListQueryHandler> logger)
    : IQueryHandler<GetPagedContractListQuery, Result<PaginatedList<ContractListItemDto>>>
{
    public async Task<Result<PaginatedList<ContractListItemDto>>> HandleAsync(
        GetPagedContractListQuery query,
        CancellationToken ct)
    {
        var totalCount = await context.Contracts.CountAsync(ct);

        var contracts = await context.Contracts
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAtUtc)
            .Skip((query.PageIndex - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(c => new ContractListItemDto
            {
                ContractId = c.Id,
                ContractNumber = c.ContractNumber,
                ContractType = c.Type
            })
            .ToListAsync(ct);

        var paginated = new PaginatedList<ContractListItemDto>(
            items: contracts,
            pageIndex: query.PageIndex,
            pageSize: query.PageSize,
            totalItemsCount: totalCount
        );

        return paginated;
    }
}
