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

    public async Task<Result<FileDownloadResult>> HandleAsync(DownloadJudgmentFileQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting file download for JudgmentId: {JudgmentId}, CaseId: {CaseId}", query.JudgmentId, query.CaseId);

        if (string.IsNullOrWhiteSpace(query.RelativePath))
        {
            _logger.LogWarning("Relative path is missing for JudgmentId: {JudgmentId}", query.JudgmentId);
            return JudgmentErrors.MissingFilePath;
        }

        var judgment = await _context.Judgments
            .AsNoTracking()
            .Include(j => j.Files)
            .FirstOrDefaultAsync(j =>
                j.Id == query.JudgmentId &&
                j.CaseId == query.CaseId &&
                j.Files.Any(x => x.Filepath == query.RelativePath),
                cancellationToken);

        if (judgment is null)
        {
            _logger.LogWarning("Judgment with ID {JudgmentId} not found for CaseId {CaseId}, or no file matches the provided path: {Path}", query.JudgmentId, query.CaseId, query.RelativePath);
            return JudgmentErrors.NotFound;
        }

        var fileResult = await _fileService.DownloadFileAsync(query.RelativePath);

        if (fileResult.IsError)
        {
            _logger.LogError("File download failed for path {Path}: {Errors}", query.RelativePath, string.Join(", ", fileResult.Errors.Select(e => e.Description)));
            return fileResult.Errors;
        }

        _logger.LogInformation("File download succeeded for JudgmentId: {JudgmentId}", query.JudgmentId);
        return fileResult.Value;
    }
}
