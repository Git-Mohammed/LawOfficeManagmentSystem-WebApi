using LiteBus.Commands.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Cases.Commands.CreateCase;
using LOMs.Application.Features.Contracts.Services;
using LOMs.Domain.Cases.Contracts;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Contracts.Commands.CreateContract;

public class CreateContractCommandHandler(IMapper mapper,
ILogger<CreateCaseCommandHandler> logger,
IAppDbContext context,
IContractFactory contractFactory,
IFileService fileService
) : ICommandHandler<CreateContractCommand, Result<Guid>>
{
    private readonly ILogger<CreateCaseCommandHandler> _logger = logger;
    private readonly IAppDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly IContractFactory _contractFactory = contractFactory;
    private readonly IFileService _fileService = fileService;

    public async Task<Result<Guid>> HandleAsync(CreateContractCommand message, CancellationToken cancellationToken = default)
    {
        var caseEntity = await _context.Cases.Include(c => c.CourtType)
            .FirstOrDefaultAsync(c => c.Id == message.CaseId, cancellationToken);

        if (caseEntity is null || caseEntity.CourtType is null)
        {
            return Error.NotFound("Case does not exist.");
        }

        if(message.ContractFiles is null || !message.ContractFiles.Any())
        {
            return Error.Validation("At least one contract file is required.");
        }

        List<ContractFile> contractFiles = new();
        List<string> filePaths = new();
        foreach (var file in message.ContractFiles)
        {
            var uploadResult = await _fileService.SaveFileAsync(file.File,file.FileName ?? string.Empty, "contracts");
            if (uploadResult.IsError)
            {
                if(filePaths.Any())
                {
                    // Rollback previously uploaded files
                    foreach(var path in filePaths)
                    {
                        await _fileService.DeleteFileAsync(path);
                    }
                }

                return uploadResult.Errors;
            }

            filePaths.Add(uploadResult.Value);


            var contractFile =  ContractFile.Create(
                file.FileName ?? string.Empty,
                uploadResult.Value
            );

            contractFiles.Add(contractFile);
        }


        var contract = await _contractFactory.CreateAsync(caseId : message.CaseId,caseEntity.CourtType, message.ContractType,message.IssuedOn,message.ExpiresOn,message.TotalAmount, message.InitialPayment,message.IsAssigned,contractFiles);

        if(contract.IsError)
        {
            return contract.Errors;
        }
        _context.Contracts.Add(contract.Value);
        await _context.SaveChangesAsync(cancellationToken);

        return contract.Value.Id;
    }
}
