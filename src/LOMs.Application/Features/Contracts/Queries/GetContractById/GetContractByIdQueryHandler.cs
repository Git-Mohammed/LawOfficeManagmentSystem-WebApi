using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Cases.Contracts;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Contracts.Queries.GetContractById;

public sealed class GetContractByIdQueryHandler
    : IQueryHandler<GetContractByIdQuery, Result<ContractDto>>
{
    private readonly ILogger<GetContractByIdQueryHandler> _logger;
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetContractByIdQueryHandler(
        ILogger<GetContractByIdQueryHandler> logger,
        IAppDbContext context,
        IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<ContractDto>> HandleAsync(
        GetContractByIdQuery request,
        CancellationToken cancellationToken)
    {
        var contractDto = await _context.ContractsWithContractNumber 
            .AsNoTracking()
            .Where(x => x.Id == request.ContractId)
            .Select(x => new ContractDto
            {
                ContractId = x.Id,
                ContractNumber = x.ContractNumber,
                ContractType = (ContractType)x.Type,
                IssueDate = x.IssuedOn,
                ExpiryDate = x.ExpiresOn,
                TotalAmount = x.BaseAmount + x.TaxAmount, 
                InitialPayment = x.InitialPayment,
                IsAssigned = x.IsAssigned
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (contractDto is null)
        {
            _logger.LogWarning("Contract with Id {ContractId} not found.", request.ContractId);
            return ContractErrors.InvalidId;
        }

        _logger.LogInformation("Contract with Id {ContractId} retrieved successfully.", request.ContractId);

        return contractDto;
    }
}
