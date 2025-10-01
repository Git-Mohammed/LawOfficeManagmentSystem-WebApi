using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Cases.POAs;
using LOMs.Domain.Common.Results;
using LOMs.Domain.POAs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.POAs.Queries.DownloadPoaFile;

public class DownloadPoaFileQueryHandler(
    ILogger<DownloadPoaFileQueryHandler> logger,
    IAppDbContext context,
    IFileService fileService
) : IQueryHandler<DownloadPoaFileQuery, Result<FileDownloadResult>>
{
    private readonly ILogger<DownloadPoaFileQueryHandler> _logger = logger;
    private readonly IAppDbContext _context = context;
    private readonly IFileService _fileService = fileService;

    public async Task<Result<FileDownloadResult>> HandleAsync(
        DownloadPoaFileQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Starting file download. PoaId={PoaId}, CaseId={CaseId}, Path={Path}",
            query.POAId, query.CaseId, query.RelativePath);

        if (string.IsNullOrWhiteSpace(query.RelativePath))
        {
            _logger.LogWarning("Relative path is missing. PoaId={PoaId}", query.POAId);
            return POAErrors.MissingFilePath;
        }

        var fileExists = await _context.POAFiles
            .AsNoTracking()
            .AnyAsync(f =>
                f.POAId == query.POAId &&
                f.POA.CaseId == query.CaseId &&
                f.Filepath == query.RelativePath,
                cancellationToken);

        if (!fileExists)
        {
            _logger.LogWarning(
                "File not found. PoaId={PoaId}, CaseId={CaseId}, Path={Path}",
                query.POAId, query.CaseId, query.RelativePath);

            return POAErrors.FileNotFound;
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
            "File download succeeded. PoaId={PoaId}, CaseId={CaseId}",
            query.POAId, query.CaseId);

        return fileResult.Value;
    }
}
