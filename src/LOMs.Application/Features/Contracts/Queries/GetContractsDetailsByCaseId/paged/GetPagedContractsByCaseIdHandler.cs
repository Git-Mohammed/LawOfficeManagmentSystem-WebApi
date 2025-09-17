using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Cases.CourtSessions;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Contracts.Queries.GetContractsDetailsByCaseId.Paged;

public sealed class GetPagedContractsByCaseIdHandler(
    IAppDbContext context,
    ILogger<GetPagedContractsByCaseIdHandler> logger)
    : IQueryHandler<GetPagedContractsByCaseIdQuery, Result<PaginatedList<ContractDetailsDto>>>
{
    public async Task<Result<PaginatedList<ContractDetailsDto>>> HandleAsync(
        GetPagedContractsByCaseIdQuery query,
        CancellationToken ct)
    {
        if (query.PageNumber < 1)
        {
            logger.LogWarning("Invalid page number: {PageNumber}", query.PageNumber);
            return Error.Validation("Pagination.InvalidPageNumber", "Page number must be at least 1.");
        }

        if (query.PageSize < 1 || query.PageSize > 100)
        {
            logger.LogWarning("Invalid page size: {PageSize}", query.PageSize);
            return Error.Validation("Pagination.InvalidPageSize", "Page size must be between 1 and 100.");
        }

        var caseExists = await context.Cases
            .AsNoTracking()
            .AnyAsync(c => c.Id == query.CaseId, ct);

        if (!caseExists)
        {
            logger.LogWarning("Fetching court sessions failed: Case not found. CaseId: {CaseId}", query.CaseId);
            return CourtSessionErrors.CaseNotFound;
        }

        var contractsQuery = context.Contracts
            .AsNoTracking()
            .Where(cs => cs.CaseId == query.CaseId);


        var totalCount = await contractsQuery.CountAsync(ct);

        var contracts = await contractsQuery
            .OrderBy(cs => cs.CreatedAtUtc)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(cs => new ContractDetailsDto
            {
                ContractId = cs.Id,
                ContractNumber = cs.ContractNumber,
                InitialPayment = cs.InitialPayment,
                IsAssigned = cs.IsAssigned,
                TotalAmount = cs.TotalAmount
            })
            .ToListAsync(ct);

        var paginated = new PaginatedList<ContractDetailsDto>(
            items: contracts,
            pageIndex: query.PageNumber,
            pageSize: query.PageSize,
            totalItemsCount: totalCount
        );

        return paginated;
    }
}



