using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Application.Features.Contracts.Queries;
using LOMs.Application.Features.Contracts.Queries.DownloadContractFile;
using LOMs.Domain.Cases.Contracts;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Contracts.Handlers;

public class DownloadContractFileQueryHandler(
    ILogger<DownloadContractFileQueryHandler> logger,
    IAppDbContext context,
    IFileService fileService
) : IQueryHandler<DownloadContractFileQuery, Result<FileDownloadResult>>
{
    private readonly ILogger<DownloadContractFileQueryHandler> _logger = logger;
    private readonly IAppDbContext _context = context;
    private readonly IFileService _fileService = fileService;

    public async Task<Result<FileDownloadResult>> HandleAsync(DownloadContractFileQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting file download for ContractId: {ContractId}, CaseId: {CaseId}", query.ContractId, query.CaseId);

        if (string.IsNullOrWhiteSpace(query.RelativePath))
        {
            _logger.LogWarning("Relative path is missing for ContractId: {ContractId}", query.ContractId);
            return ContractErrors.MissingFilePath;
        }

        var contract = await _context.Contracts
            .AsNoTracking()
            .Include(c => c.ContractFiles)
            .FirstOrDefaultAsync(c =>
                c.Id == query.ContractId &&
                c.CaseId == query.CaseId &&
                c.ContractFiles.Any(x => x.Filepath == query.RelativePath),
                cancellationToken);
        if (contract is null)
        {
            _logger.LogWarning("Contract not found or does not belong to CaseId: {CaseId}", query.CaseId);
            return ContractErrors.InvalidId;
        }


        var fileResult = await _fileService.DownloadFileAsync(query.RelativePath);

        if (fileResult.IsError)
        {
            _logger.LogError("File download failed for path {Path}: {Errors}", query.RelativePath, string.Join(", ", fileResult.Errors.Select(e => e.Description)));
            return fileResult.Errors;
        }

        _logger.LogInformation("File download succeeded for ContractId: {ContractId}", query.ContractId);
        return fileResult.Value;
    }
}
