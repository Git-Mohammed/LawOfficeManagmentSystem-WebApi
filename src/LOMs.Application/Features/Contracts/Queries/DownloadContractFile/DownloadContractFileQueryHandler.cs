using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Cases.Contracts;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Contracts.Queries.DownloadContractFile;

public class DownloadContractFileQueryHandler(
    ILogger<DownloadContractFileQueryHandler> logger,
    IAppDbContext context,
    IFileService fileService
) : IQueryHandler<DownloadContractFileQuery, Result<FileDownloadResult>>
{
    private readonly ILogger<DownloadContractFileQueryHandler> _logger = logger;
    private readonly IAppDbContext _context = context;
    private readonly IFileService _fileService = fileService;

    public async Task<Result<FileDownloadResult>> HandleAsync(
        DownloadContractFileQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Starting file download. ContractId={ContractId}, CaseId={CaseId}, Path={Path}",
            query.ContractId, query.CaseId, query.RelativePath);

        if (string.IsNullOrWhiteSpace(query.RelativePath))
        {
            _logger.LogWarning("Relative path is missing. ContractId={ContractId}", query.ContractId);
            return ContractErrors.MissingFilePath;
        }

        var fileExists = await _context.ContractFiles
            .AsNoTracking()
            .AnyAsync(f =>
                f.ContractId == query.ContractId &&
                f.Contract!.CaseId == query.CaseId &&
                f.Filepath == query.RelativePath,
                cancellationToken);

        if (!fileExists)
        {
            _logger.LogWarning(
                "File not found. ContractId={ContractId}, CaseId={CaseId}, Path={Path}",
                query.ContractId, query.CaseId, query.RelativePath);

            return ContractErrors.FileNotFound;
        }

        var fileResult = await _fileService.DownloadFileAsync(query.RelativePath);

        if (fileResult.IsError)
        {
            _logger.LogError(
                "File download failed. Path={Path}, Errors={Errors}",
                query.RelativePath,
                string.Join(", ", fileResult.Errors.Select(e => e.Description)));

            return fileResult.Errors;
        }

        _logger.LogInformation(
            "File download succeeded. ContractId={ContractId}, CaseId={CaseId}",
            query.ContractId, query.CaseId);

        return fileResult.Value;
    }
}
