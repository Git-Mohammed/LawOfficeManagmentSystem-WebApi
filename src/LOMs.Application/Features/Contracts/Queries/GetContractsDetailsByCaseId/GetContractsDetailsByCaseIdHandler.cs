using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Cases;
using LOMs.Domain.Cases.Contracts;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Contracts.Queries;

public class GetContractsDetailsByCaseIdHandler
    : IQueryHandler<GetContractsDetailsByCaseIdQuery, Result<List<ContractDetailsDto>>>
{
    private readonly IAppDbContext _context;
    private readonly ILogger<GetContractsDetailsByCaseIdHandler> _logger;
    private readonly IMapper _mapper;

    public GetContractsDetailsByCaseIdHandler(
        IAppDbContext context,
        IMapper mapper,
        ILogger<GetContractsDetailsByCaseIdHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<List<ContractDetailsDto>>> HandleAsync(
        GetContractsDetailsByCaseIdQuery query,
        CancellationToken ct)
    {

        var caseExsting = await _context.Cases.AnyAsync(x => x.Id  == query.CaseId);

        if (!caseExsting)
        {
            return CaseErrors.CaseNotFound;
        }

        var contracts = await _context.Contracts
            .AsNoTracking()
            .Where(c => c.CaseId == query.CaseId)
            .Select(c => new ContractDetailsDto
            {
                ContractId = c.Id,
                ContractNumber = c.ContractNumber,
                TotalAmount = c.BaseAmount + c.TaxAmount,
                InitialPayment = c.InitialPayment,
                IsAssigned = c.IsAssigned,

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
