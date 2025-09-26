using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Contracts.Queries.GetContractList;

public sealed class GetContractListQueryHandler
    : IQueryHandler<GetContractListQuery, Result<List<ContractListItemDto>>>
{
    private readonly IAppDbContext _context;
    private readonly ILogger<GetContractListQueryHandler> _logger;

    public GetContractListQueryHandler(
        IAppDbContext context,
        ILogger<GetContractListQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<List<ContractListItemDto>>> HandleAsync(
        GetContractListQuery query,
        CancellationToken ct)
    {
        var contracts = await (
            from contract in _context.ContractsWithContractNumber.AsNoTracking()
            join employee in _context.Employees.AsNoTracking()
                on contract.CreatedBy equals employee.UserId into empGroup
            from emp in empGroup.DefaultIfEmpty()
            select new ContractListItemDto
            {
                ContractId = contract.Id,
                ContractNumber = contract.ContractNumber, 
                ContractType =(ContractType) contract.Type,
                IssueDate = contract.IssuedOn,
                ExpiryDate = contract.ExpiresOn,
                TotalAmount = contract.BaseAmount + contract.TaxAmount,
                InitialPayment = contract.InitialPayment,
                IsAssigned = contract.IsAssigned,
                EmployeeCreatedBy = emp.Person.FullName ?? string.Empty,
                CreatedDate = contract.CreatedAtUtc
            })
            .ToListAsync(ct);

        if (contracts.Count == 0)
        {
            _logger.LogInformation("No contracts found for query {Query}.", query);
        }
        else
        {
            _logger.LogInformation("{Count} contracts retrieved successfully.", contracts.Count);
        }

        return contracts;
    }
}
