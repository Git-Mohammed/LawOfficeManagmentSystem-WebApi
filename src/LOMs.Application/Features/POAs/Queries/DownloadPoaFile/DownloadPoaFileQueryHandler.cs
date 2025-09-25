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

    public async Task<Result<FileDownloadResult>> HandleAsync(DownloadPoaFileQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting file download for PoaId: {PoaId}, CaseId: {CaseId}", query.POAId, query.CaseId);

        if (string.IsNullOrWhiteSpace(query.RelativePath))
        {
            _logger.LogWarning("Relative path is missing for PoaId: {PoaId}", query.POAId);
            return POAErrors.MissingFilePath;
        }

        var poa = await _context.POAs
            .AsNoTracking()
            .Include(p => p.POAFiles)
            .FirstOrDefaultAsync(p =>
                p.Id == query.POAId &&
                p.CaseId == query.CaseId &&
                p.POAFiles.Any(x => x.Filepath == query.RelativePath),
                cancellationToken);

        if (poa is null)
        {
            _logger.LogWarning("POA with ID {PoaId} not found for CaseId {CaseId}, or no file matches the provided path: {Path}", query.POAId, query.CaseId, query.RelativePath);
            return POAErrors.InvalidId;
        }

        var fileResult = await _fileService.DownloadFileAsync(query.RelativePath);

        if (fileResult.IsError)
        {
            _logger.LogError("File download failed for path {Path}: {Errors}", query.RelativePath, string.Join(", ", fileResult.Errors.Select(e => e.Description)));
            return fileResult.Errors;
        }

        _logger.LogInformation("File download succeeded for PoaId: {PoaId}", query.POAId);
        return fileResult.Value;
    }
}
