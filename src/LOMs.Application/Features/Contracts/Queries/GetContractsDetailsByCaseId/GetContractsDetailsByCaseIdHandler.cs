using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Cases;
using LOMs.Domain.Cases.Contracts;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Contracts.Queries;

public sealed class GetContractsDetailsByCaseIdHandler
    : IQueryHandler<GetContractsDetailsByCaseIdQuery, Result<List<ContractDetailsDto>>>
{
    private readonly IAppDbContext _context;
    private readonly ILogger<GetContractsDetailsByCaseIdHandler> _logger;

    public GetContractsDetailsByCaseIdHandler(
        IAppDbContext context,
        ILogger<GetContractsDetailsByCaseIdHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<List<ContractDetailsDto>>> HandleAsync(
        GetContractsDetailsByCaseIdQuery query,
        CancellationToken ct)
    {
        var caseExists = await _context.Cases
            .AsNoTracking()
            .AnyAsync(x => x.Id == query.CaseId, ct);

        if (!caseExists)
        {
            _logger.LogWarning("Case not found: {CaseId}", query.CaseId);
            return CaseErrors.CaseNotFound;
        }

        var contractsQuery = _context.ContractsWithContractNumber
            .AsNoTracking()
            .Where(c => c.CaseId == query.CaseId);

        var contracts = await (
            from contract in contractsQuery
            join employee in _context.Employees.AsNoTracking()
                on contract.CreatedBy equals employee.UserId into empGroup
            from emp in empGroup.DefaultIfEmpty()
            join file in _context.ContractFiles.AsNoTracking()
                on contract.Id equals file.ContractId into fileGroup
            orderby contract.CreatedAtUtc descending
            select new ContractDetailsDto
            {
                ContractId = contract.Id,
                CaseId = contract.CaseId,
                ContractNumber = contract.ContractNumber,
                ContractType = (ContractType)contract.Type,
                IssueDate = contract.IssuedOn,
                ExpiryDate = contract.ExpiresOn,
                TotalAmount = contract.BaseAmount + contract.TaxAmount,
                FilePath = fileGroup.Select(f => f.Filepath).FirstOrDefault() ?? string.Empty,
                EmployeeIdCreatedBy = emp.Id,
                EmployeeNameCreatedBy = emp.Person.FullName ?? "Unknown",
                CreatedDate = contract.CreatedAtUtc
            })
            .ToListAsync(ct);

        if (!contracts.Any())
        {
            _logger.LogWarning("No contracts found for CaseId: {CaseId}", query.CaseId);
            return ContractErrors.ContractsNotFoundByCaseId;
        }

        return contracts;
    }
}
