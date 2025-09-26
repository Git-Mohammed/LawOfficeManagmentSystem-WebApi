using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Contracts.Queries.GetContractList.Paged;

public sealed class GetPagedContractListQueryHandler
    : IQueryHandler<GetPagedContractListQuery, Result<PaginatedList<ContractListItemDto>>>
{
    private readonly IAppDbContext _context;
    private readonly ILogger<GetPagedContractListQueryHandler> _logger;

    public GetPagedContractListQueryHandler(
        IAppDbContext context,
        ILogger<GetPagedContractListQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<PaginatedList<ContractListItemDto>>> HandleAsync(
        GetPagedContractListQuery query,
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


        var totalCount = await _context.ContractsWithContractNumber.CountAsync(ct);

        var contracts = await (
            from contract in _context.ContractsWithContractNumber.AsNoTracking()
            join employee in _context.Employees.AsNoTracking()
                on contract.CreatedBy equals employee.UserId into empGroup
            from emp in empGroup.DefaultIfEmpty()
            orderby contract.CreatedAtUtc descending
            select new ContractListItemDto
            {
                ContractId = contract.Id,
                ContractNumber = contract.ContractNumber, 
                ContractType =(ContractType)contract.Type,
                IssueDate = contract.IssuedOn,
                ExpiryDate = contract.ExpiresOn,
                TotalAmount = contract.BaseAmount + contract.TaxAmount, 
                InitialPayment = contract.InitialPayment,
                IsAssigned = contract.IsAssigned,
                EmployeeCreatedBy = emp.Person.FullName ?? string.Empty,
                CreatedDate = contract.CreatedAtUtc
            })
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(ct);

        var paginated = new PaginatedList<ContractListItemDto>(
            items: contracts,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            totalItemsCount: totalCount
        );

        _logger.LogInformation("{Count} contracts retrieved for page {PageNumber}.", contracts.Count, query.PageNumber);

        return paginated;
    }
}
