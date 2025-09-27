using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Cases.Attachments;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Attachments.Queries.DownloadAttachmentFile;

public sealed class DownloadAttachmentFileQueryHandler(
    ILogger<DownloadAttachmentFileQueryHandler> logger,
    IFileService fileService,
    IAppDbContext context
) : IQueryHandler<DownloadAttachmentFileQuery, Result<FileDownloadResult>>
{
    private readonly ILogger<DownloadAttachmentFileQueryHandler> _logger = logger;
    private readonly IAppDbContext _context = context;
    private readonly IFileService _fileService = fileService;

    public async Task<Result<FileDownloadResult>> HandleAsync(
        DownloadAttachmentFileQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Starting file download request. CaseId={CaseId}, AttachmentId={AttachmentId}, Path={Path}",
            query.CaseId,
            query.AttachmentId,
            query.FilePath);

        if (string.IsNullOrWhiteSpace(query.FilePath))
        {
            _logger.LogWarning(
                "File download aborted: missing or empty file path. CaseId={CaseId}, AttachmentId={AttachmentId}",
                query.CaseId,
                query.AttachmentId);

            return AttachmentErrors.MissingFilePath;
        }

        var fileExists = await _context.Attachments
            .AsNoTracking()
            .AnyAsync(f =>
                f.CaseId == query.CaseId &&
                f.Id == query.AttachmentId &&
                f.Filepath == query.FilePath,
                cancellationToken);

        if (!fileExists)
        {
            _logger.LogWarning(
                "File not found in database. CaseId={CaseId}, AttachmentId={AttachmentId}, Path={Path}. " +
                "Possible reasons: attachment was deleted, path mismatch, or wrong case reference.",
                query.CaseId,
                query.AttachmentId,
                query.FilePath);

            return AttachmentErrors.FileNotFound;
        }

        _logger.LogInformation(
            "Database record found. Proceeding to download file. CaseId={CaseId}, AttachmentId={AttachmentId}, Path={Path}",
            query.CaseId,
            query.AttachmentId,
            query.FilePath);

        var fileResult = await _fileService.DownloadFileAsync(query.FilePath);

        if (fileResult.IsError)
        {
            _logger.LogError(
                "File service failed to download. CaseId={CaseId}, AttachmentId={AttachmentId}, Path={Path}, ErrorCount={ErrorCount}, Errors={Errors}",
                query.CaseId,
                query.AttachmentId,
                query.FilePath,
                fileResult.Errors.Count,
                string.Join("; ", fileResult.Errors.Select(e => $"{e.Code}:{e.Description}")));

            return fileResult.Errors;
        }

        _logger.LogInformation(
            "File download succeeded. CaseId={CaseId}, AttachmentId={AttachmentId}, Path={Path}, FileName={FileName}, Size={Size} bytes",
            query.CaseId,
            query.AttachmentId,
            query.FilePath,
            fileResult.Value.FileName,
            fileResult.Value.FileStream.Length);

        return fileResult.Value;
    }
}
