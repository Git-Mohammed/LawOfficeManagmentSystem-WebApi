using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Cases.Contracts;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Contracts.Queries.GetContractsDetailsByCaseId.Paged;

public sealed class GetPagedContractsByCaseIdHandler
    : IQueryHandler<GetPagedContractsByCaseIdQuery, Result<PaginatedList<ContractDetailsDto>>>
{
    private readonly IAppDbContext _context;
    private readonly ILogger<GetPagedContractsByCaseIdHandler> _logger;

    public GetPagedContractsByCaseIdHandler(
        IAppDbContext context,
        ILogger<GetPagedContractsByCaseIdHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<PaginatedList<ContractDetailsDto>>> HandleAsync(
        GetPagedContractsByCaseIdQuery query,
        CancellationToken ct)
    {
        if (query.PageNumber < 1)
        {
            _logger.LogWarning("Invalid page number: {PageNumber}", query.PageNumber);
            return Error.Validation("Pagination.InvalidPageNumber", "Page number must be at least 1.");
        }

        if (query.PageSize < 1 || query.PageSize > 100)
        {
            _logger.LogWarning("Invalid page size: {PageSize}", query.PageSize);
            return Error.Validation("Pagination.InvalidPageSize", "Page size must be between 1 and 100.");
        }

        var caseExists = await _context.Cases
            .AsNoTracking()
            .AnyAsync(c => c.Id == query.CaseId, ct);

        if (!caseExists)
        {
            _logger.LogWarning("Fetching contracts failed: Case not found. CaseId: {CaseId}", query.CaseId);
            return ContractErrors.InvalidCaseId;
        }

        var contractsQuery = _context.ContractsWithContractNumber
            .AsNoTracking()
            .Where(c => c.CaseId == query.CaseId);

        var totalCount = await contractsQuery.CountAsync(ct);

        var contracts = await (
            from contract in contractsQuery
            join employee in _context.Employees.AsNoTracking()
                on contract.CreatedBy equals employee.UserId into empGroup
            from emp in empGroup.DefaultIfEmpty()
            orderby contract.CreatedAtUtc descending
            select new ContractDetailsDto
            {
                ContractId = contract.Id,
                ContractNumber = contract.ContractNumber,
                ContractType = (ContractType)contract.Type,
                TotalAmount = contract.BaseAmount + contract.TaxAmount,
                InitialPayment = contract.InitialPayment,
                IsAssigned = contract.IsAssigned,
                CreatedDate = contract.CreatedAtUtc
            })
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(ct);

        var paginated = new PaginatedList<ContractDetailsDto>(
            items: contracts,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            totalItemsCount: totalCount
        );

        return paginated;
    }
}
