using LiteBus.Commands.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Domain.Cases;
using LOMs.Domain.Cases.Judgments;
using LOMs.Domain.Common.Files;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Judgments.Commands;

public class CreateJudgmentCommandHandler(IAppDbContext context, IMapper mapper, ILogger<CreateJudgmentCommandHandler> logger, IFileService fileService) : ICommandHandler<CreateJudgmentCommand, Result<Guid>>
{
    private readonly IAppDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CreateJudgmentCommandHandler> _logger = logger;
    private readonly IFileService _fileService = fileService;

    public async Task<Result<Guid>> HandleAsync(CreateJudgmentCommand command, CancellationToken cancellationToken = new CancellationToken())
    {
        var isExistingCaseEntity = await _context.Cases.AnyAsync(c => c.Id == command.CaseId, cancellationToken);

        if (!isExistingCaseEntity)
        {
            _logger.LogWarning("Case with Id {CaseId} not found.", command.CaseId);
            return CaseErrors.CaseNotFound;
        }
        
        // save Judgment File
        var savedFile = await _fileService.SaveFileAsync(command.FileCommand.File, command.FileCommand.FileName, nameof(FileType.Judgment));
        if (savedFile.IsError)
            return savedFile.Errors;
        
        // create Judgment entity
        var judgment = Judgment.Create(
            caseId: command.CaseId,
            number: command.Number,
            type: command.Type,
            subType: command.SubType,
            issuedOn: command.IssuedOn
        );
        
        if (judgment.IsError)
            return judgment.Errors;
        
        await _context.Judgments.AddAsync(judgment.Value, cancellationToken);
        
        // create JudgmentFile entity
        var fileResult = JudgmentFile.Create(
            judgmentId: judgment.Value.Id,
            fileName: command.FileCommand.FileName,
            filePath: savedFile.Value,
            fileType: FileType.Judgment,
            size: command.FileCommand.File.Length,
            extension: Path.GetExtension(command.FileCommand.FileName),
            contentType: command.FileCommand.ContentType
        );

        if (fileResult.IsError)
            return fileResult.Errors;
        
        await _context.JudgmentFiles.AddAsync(fileResult.Value, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        return judgment.Value.Id;
        
    }
}