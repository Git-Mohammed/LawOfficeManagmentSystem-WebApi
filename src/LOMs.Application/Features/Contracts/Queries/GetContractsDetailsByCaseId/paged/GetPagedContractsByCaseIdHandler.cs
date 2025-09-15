using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Cases;
using LOMs.Domain.Cases.Contracts;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Contracts.Queries.GetContractsDetailsByCaseId.GetPagedContractsByCaseId;
public class GetPagedContractsByCaseIdHandler
    : IQueryHandler<GetPagedContractsByCaseIdQuery, Result<PaginatedList<ContractDetailsDto>>>
{
    private readonly IAppDbContext _context;
    private readonly ILogger<GetPagedContractsByCaseIdHandler> _logger;

    public GetPagedContractsByCaseIdHandler(IAppDbContext context, ILogger<GetPagedContractsByCaseIdHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<PaginatedList<ContractDetailsDto>>> HandleAsync(
        GetPagedContractsByCaseIdQuery query,
        CancellationToken ct)
    {
        var caseExsting = await _context.Cases.AnyAsync(x => x.Id == query.CaseId);

        if (!caseExsting)
        {
            return CaseErrors.CaseNotFound;
        }

        var totalItemsCount = await _context.Contracts
            .Where(c => c.CaseId == query.CaseId)
            .CountAsync(ct);

        if (totalItemsCount == 0)
        {
            _logger.LogWarning("No contracts found for CaseId: {CaseId}", query.CaseId);
            return ContractErrors.ContractsNotFoundByCaseId;
        }

        var contracts = await _context.Contracts
            .AsNoTracking()
            .Where(c => c.CaseId == query.CaseId)
            .OrderByDescending(c => c.CreatedAtUtc)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(c => new ContractDetailsDto
            {
                ContractId = c.Id,
                ContractNumber = c.ContractNumber,
                TotalAmount = c.BaseAmount + c.TaxAmount,
                InitialPayment = c.InitialPayment,
                IsAssigned = c.IsAssigned
            })
            .ToListAsync(ct);

        var pagedResult = new PaginatedList<ContractDetailsDto>(
            items: contracts,
            pageIndex: query.PageNumber,
            pageSize: query.PageSize,
            totalItemsCount: totalItemsCount
        );

        return pagedResult;
    }
}

