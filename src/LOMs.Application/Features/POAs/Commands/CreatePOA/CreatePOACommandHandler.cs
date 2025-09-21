using LiteBus.Commands.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Contracts.Services;
using LOMs.Domain.Cases.Contracts;
using LOMs.Domain.Cases.POAs;
using LOMs.Domain.Common.Files;
using LOMs.Domain.Common.Results;
using LOMs.Domain.POAs;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LOMs.Application.Features.POAs.Commands.CreatePOA
{
    public sealed class CreatePOACommandHandler(
    ILogger<CreatePOACommandHandler> logger,
    IAppDbContext context,
    IContractFactory contractFactory,
    IFileService fileService
) : ICommandHandler<CreatePOACommand, Result<Guid>>
    {
        private readonly ILogger<CreatePOACommandHandler> _logger = logger;
        private readonly IAppDbContext _context = context;
        private readonly IContractFactory _contractFactory = contractFactory;
        private readonly IFileService _fileService = fileService;

        public async Task<Result<Guid>> HandleAsync(CreatePOACommand command, CancellationToken cancellationToken = default)
        {
            var caseExstists = _context.Cases.Any(c => c.Id == command.CaseId);
            if (!caseExstists)
            {
                _logger.LogWarning("Case with ID {CaseId} does not exist.", command.CaseId);
                return POAErrors.InvalidCaseId(command.CaseId);
            }

            var filepath = await _fileService.SaveFileAsync(
                            command.POAFile.File,
                            command.POAFile.FileName,
                            nameof(FileType.POA));


            if(filepath.IsError)
            {
                _logger.LogError("Failed to save POA file for Case ID {CaseId}.", command.CaseId);
                return Error.Failure("Failed to save POA file.");
            }

                var poaFileResult = POAFile.Create(
                        command.POAFile.FileName,
                        filepath.Value,
                        FileType.POA,
                        command.POAFile.File.Length,
                        Path.GetExtension(command.POAFile.FileName),
                        command.POAFile.ContentType);

               if (poaFileResult.IsError)
            {
                _logger.LogError("Failed to create POA file entity for Case ID {CaseId}. Errors: {Errors}", command.CaseId, string.Join(", ", poaFileResult.Errors.Select(e => e.Description)));
                return poaFileResult.Errors;
            }

           var poa = POA.Create(
                command.CaseId,
                command.POANumber,
                command.IssueDate,
                command.IssuingAuthority,
                new List<POAFile> { poaFileResult.Value }
            );
            if (poa.IsError)
            {
                _logger.LogError("Failed to create POA entity for Case ID {CaseId}. Errors: {Errors}", command.CaseId, string.Join(", ", poa.Errors.Select(e => e.Description)));
                return poa.Errors;
            }
             _context.POAs.Add(poa.Value);
            var saveResult = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (!saveResult)
            {
                _logger.LogError("Failed to save POA entity to the database for Case ID {CaseId}.", command.CaseId);
                return Error.Failure("Failed to save POA.");
            }
            _logger.LogInformation("Successfully created POA with ID {POAId} for Case ID {CaseId}.", poa.Value.Id, command.CaseId);
            return poa.Value.Id;

        }
    }
}
