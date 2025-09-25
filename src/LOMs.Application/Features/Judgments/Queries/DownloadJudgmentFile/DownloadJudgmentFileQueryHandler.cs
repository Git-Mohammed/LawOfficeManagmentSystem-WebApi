using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Cases.Judgments;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Judgments.Queries.DownloadJudgmentFile;

public class DownloadJudgmentFileQueryHandler(
    ILogger<DownloadJudgmentFileQueryHandler> logger,
    IAppDbContext context,
    IFileService fileService
) : IQueryHandler<DownloadJudgmentFileQuery, Result<FileDownloadResult>>
{
    private readonly ILogger<DownloadJudgmentFileQueryHandler> _logger = logger;
    private readonly IAppDbContext _context = context;
    private readonly IFileService _fileService = fileService;

    public async Task<Result<FileDownloadResult>> HandleAsync(
        DownloadJudgmentFileQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Starting file download. JudgmentId={JudgmentId}, CaseId={CaseId}, Path={Path}",
            query.JudgmentId, query.CaseId, query.RelativePath);

        if (string.IsNullOrWhiteSpace(query.RelativePath))
        {
            _logger.LogWarning("Relative path is missing. JudgmentId={JudgmentId}", query.JudgmentId);
            return JudgmentErrors.MissingFilePath;
        }

        var fileExists = await _context.JudgmentFiles
            .AsNoTracking()
            .AnyAsync(f =>
                f.JudgmentId == query.JudgmentId &&
                f.Judgment.CaseId == query.CaseId &&
                f.Filepath == query.RelativePath,
                cancellationToken);

        if (!fileExists)
        {
            _logger.LogWarning(
                "File not found. JudgmentId={JudgmentId}, CaseId={CaseId}, Path={Path}",
                query.JudgmentId, query.CaseId, query.RelativePath);

            return JudgmentErrors.FileNotFound;
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
            "File download succeeded. JudgmentId={JudgmentId}, CaseId={CaseId}",
            query.JudgmentId, query.CaseId);

        return fileResult.Value;
    }
}
