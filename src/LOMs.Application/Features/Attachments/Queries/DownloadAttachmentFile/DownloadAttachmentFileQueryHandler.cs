using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Application.Features.Judgments.Queries.DownloadJudgmentFile;
using LOMs.Domain.Cases.Attachments;
using LOMs.Domain.Cases.Judgments;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Application.Features.Attachments.Queries.DownloadAttachmentFile
{
    public class DownloadAttachmentFileQueryHandler(
        ILogger<DownloadAttachmentFileQueryHandler> logger,
         IFileService fileService,
         IAppDbContext context) : IQueryHandler<DownloadAttachmentFileQuery, Result<FileDownloadResult>>

    {
        private readonly ILogger<DownloadAttachmentFileQueryHandler> _logger = logger;
        private readonly IAppDbContext _context = context;
        private readonly IFileService _fileService = fileService;

        public async Task<Result<FileDownloadResult>> HandleAsync(DownloadAttachmentFileQuery query, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
           "Starting file download. , Path={Path}",
            query.FilePath);

            if (string.IsNullOrWhiteSpace(query.FilePath))
            {
                _logger.LogWarning("Relative path is missing. CaseId={CaseId}" , query.CaseId);
                return AttachmentErrors.MissingFilePath;
            }
            var fileExists = await _context.Attachments
           .AsNoTracking()
           .AnyAsync(f =>
               f.CaseId == query.CaseId &&
               f.Filepath == query.FilePath,
               cancellationToken);

            if (!fileExists)
            {
                _logger.LogWarning(
                    "File not found. CaseId={CaseId}, Path={Path}",query.CaseId, query.FilePath);

                return AttachmentErrors.FileNotFound;
            }
                var fileResult = await _fileService.DownloadFileAsync(query.FilePath);

                if (fileResult.IsError)
                {
                    _logger.LogError(
                        "File download failed. Path={Path}, Errors={Errors}",
                        query.FilePath,
                        string.Join(", ", fileResult.Errors.Select(e => e.Description)));

                    return fileResult.Errors;
                }

            _logger.LogInformation("File download succeeded.  CaseId={CaseId}", query.CaseId);
      

            return fileResult.Value;
        }
        }
    }

