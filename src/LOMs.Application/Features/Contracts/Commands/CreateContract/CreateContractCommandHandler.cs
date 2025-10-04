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
        _logger.LogInformation("Starting contract creation for CaseId: {CaseId}", command.CaseId);

        if (command?.ContractFile is null)
        {
            _logger.LogWarning("Contract file is missing in the command for CaseId: {CaseId}", command?.CaseId);
            return ContractErrors.MissingFilePath;
        }
        var caseEntity = await _context.Cases
            .Include(c => c.CourtType)
            .FirstOrDefaultAsync(c => c.Id == command.CaseId, cancellationToken);

        if (caseEntity is null)
        {
            _logger.LogWarning("Case not found or CourtType is null for CaseId: {CaseId}", command.CaseId);
            return ContractErrors.InvalidCaseId;
        }

        if (caseEntity.CourtType is null)
        {
            _logger.LogWarning("CourtType is null for CaseId: {CaseId}", command.CaseId);
            return ContractErrors.InvalidCourtType;
        }


        var contract =  _contractFactory.CreateAsync(
            caseEntity.Id,
            caseEntity.CourtType,
            command.ContractType,
            command.IssuedOn,
            command.ExpiresOn,
            command.TotalAmount,
            command.InitialPayment,
            command.IsAssigned);

        if (contract.IsError)
        {
            _logger.LogWarning("Contract creation failed: {Errors}", string.Join(", ", contract.Errors.Select(e => e.Description)));
       
            return contract.Errors;
        }

        var contractEntity = contract.Value;

        var filepath = await _fileService.SaveFileAsync(
            command.ContractFile.File,
            command.ContractFile.FileName,
            nameof(FileType.Contract));

        if (filepath.IsError)
        {
            _logger.LogError("File saving failed: {Errors}", string.Join(", ", filepath.Errors.Select(e => e.Description)));
            return ContractErrors.UnexpectedFailure; // or a specific FileSaveFailed error if you add one

        }

        var contractFile = ContractFile.Create(
            contractEntity.Id,
            command.ContractFile.FileName,
            filepath.Value,
            FileType.Contract,
            command.ContractFile.File.Length,
            Path.GetExtension(command.ContractFile.FileName),
            command.ContractFile.ContentType);

        if (contractFile.IsError)
        {
            await _fileService.DeleteFileAsync(filepath.Value);
            _logger.LogWarning("ContractFile creation failed: {Errors}", string.Join(", ", contractFile.Errors.Select(e => e.Description)));
            return contractFile.Errors;

        }

        try
        {
            _context.Contracts.Add(contractEntity);
            _context.ContractFiles.Add(contractFile.Value);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating contract.");
            await _fileService.DeleteFileAsync(filepath.Value);
            return ContractErrors.UnexpectedFailure;
        }

        return contractEntity.Id;
    }
}
