using LiteBus.Commands.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Contracts.Services;
using LOMs.Domain.Cases.Contracts;
using LOMs.Domain.Common.Files;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Contracts.Commands.CreateContract;

public class CreateContractCommandHandler(
    ILogger<CreateContractCommandHandler> logger,
    IAppDbContext context,
    IContractFactory contractFactory,
    IFileService fileService
) : ICommandHandler<CreateContractCommand, Result<Guid>>
{
    private readonly ILogger<CreateContractCommandHandler> _logger = logger;
    private readonly IAppDbContext _context = context;
    private readonly IContractFactory _contractFactory = contractFactory;
    private readonly IFileService _fileService = fileService;

    public async Task<Result<Guid>> HandleAsync(CreateContractCommand command, CancellationToken cancellationToken = default)
    {
        var caseEntity = await _context.Cases
            .Include(c => c.CourtType)
            .FirstOrDefaultAsync(c => c.Id == command.CaseId, cancellationToken);

        if (caseEntity?.CourtType is null)
            return Error.NotFound("Case does not exist.");

        if (command?.ContractFile is null)
            return Error.Validation("Contract file is required.");

        var contract = await _contractFactory.CreateAsync(
            caseEntity.Id,
            caseEntity.CourtType,
            command.ContractType,
            command.IssuedOn,
            command.ExpiresOn,
            command.TotalAmount,
            command.InitialPayment,
            command.IsAssigned);

        if (contract.IsError)
            return contract.Errors;

        var contractEntity = contract.Value;

        var filepath = await _fileService.SaveFileAsync(
            command.ContractFile.File,
            command.ContractFile.FileName,
            nameof(FileType.Contract));

        if (filepath.IsError)
            return Error.Failure("Failed to save contract file.");

        var contractFile = ContractFile.Create(
            contractEntity.Id,
            command.ContractFile.FileName,
            filepath.Value,
            FileType.Contract,
            command.ContractFile.File.Length,
            Path.GetExtension(command.ContractFile.FileName),
            command.ContractFile.ContentType);
        if(contractFile.IsError)
            return contractFile.Errors;
        
        _context.Contracts.Add(contractEntity);
        _context.ContractFiles.Add(contractFile.Value);

        await _context.SaveChangesAsync(cancellationToken);

        return contractEntity.Id;
    }
}