using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Application.Features.Templates.DTOs;
using LOMs.Domain.Cases.Contracts;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Contracts.Queries.GetContractsDetailsByCaseId.Paged;

public sealed class GetPagedContractsDetailsByCaseIdHandler
    : IQueryHandler<GetPagedContractsDetailsByCaseIdQuery, Result<PaginatedList<ContractDetailsDto>>>
{
    private readonly IAppDbContext _context;
    private readonly ILogger<GetPagedContractsDetailsByCaseIdHandler> _logger;

    public GetPagedContractsDetailsByCaseIdHandler(
        IAppDbContext context,
        ILogger<GetPagedContractsDetailsByCaseIdHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<PaginatedList<ContractDetailsDto>>> HandleAsync(
        GetPagedContractsDetailsByCaseIdQuery query,
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
           Files = fileGroup.Select(f => new FileDto { FilePath = f.Filepath, FileName = f.Filename, FileType = f.ContentType }).ToList(),
           EmployeeIdCreatedBy = emp.Id,
           EmployeeNameCreatedBy = emp.Person.FullName ?? "Unknown",
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
