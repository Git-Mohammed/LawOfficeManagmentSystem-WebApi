using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Contracts.Queries.GetContractList;

public sealed class GetContractListQueryHandler(
    IAppDbContext context,
    ILogger<GetContractListQueryHandler> logger)
    : IQueryHandler<GetContractListQuery, Result<List<ContractListItemDto>>>
{
    public async Task<Result<List<ContractListItemDto>>> HandleAsync(
        GetContractListQuery query,
        CancellationToken ct)
    {
        var contracts = await context.Contracts
            .AsNoTracking()
            .Select(c => new ContractListItemDto
            {
                ContractId = c.Id,
                ContractNumber = c.ContractNumber,
                ContractType = c.Type 
            })
            .ToListAsync(ct);

        return contracts;
    }
}
