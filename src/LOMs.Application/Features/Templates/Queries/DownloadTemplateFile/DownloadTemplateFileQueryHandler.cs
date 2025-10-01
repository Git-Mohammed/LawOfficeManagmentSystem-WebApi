using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Attachments.Queries.DownloadAttachmentFile;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Cases.Attachments;
using LOMs.Domain.Common.Results;
using LOMs.Domain.Templates;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Application.Features.Templates.Queries.DownloadTemplateFile
{
    public sealed class DownloadTemplateFileQueryHandler(
        ILogger<DownloadTemplateFileQueryHandler> logger,
        IAppDbContext context,
        IFileService fileService) : IQueryHandler<DownloadTemplateFileQuery, Result<FileDownloadResult>>
    {
        private readonly ILogger<DownloadTemplateFileQueryHandler> _logger = logger;
        private readonly IAppDbContext _context = context;
        private readonly IFileService _fileService = fileService;
        public async Task<Result<FileDownloadResult>> HandleAsync(DownloadTemplateFileQuery query, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
           "Starting file download request. TemplateId={TemplateId}, Path={Path}",
           query.templateId,
           query.filepath);

             if (string.IsNullOrWhiteSpace(query.filepath))
        {
              _logger.LogWarning(
                "File download aborted: missing or empty file path. filePath={Path}",
                query.filepath);
                return TemplateErrors.MissingFilePath;


        }
            var fileExists = await _context.Templates
                 .AsNoTracking()
                .AnyAsync(f =>
              f.Id == query.templateId &&
              f.Filepath == query.filepath,
               cancellationToken);

            if (!fileExists)
            {
                _logger.LogWarning(
                    "File not found in database. ,TemplateId={TemplateId}, Path={Path}. " +
                    "Possible reasons: template was deleted, path mismatch, or wrong case reference.",
                    query.templateId,
                    query.filepath);

                return TemplateErrors.FileNotFound;
            }


            _logger.LogInformation(
             "Database record found. Proceeding to download file. TemplateId={TemplateId}, Path={Path}",
             query.templateId,
             query.filepath);

            var fileResult = await _fileService.DownloadFileAsync(query.filepath);

            if (fileResult.IsError)
            {
               _logger.LogError(
                    "File service failed to download.TemplateId={TemplateId}, Path={Path}, ErrorCount={ErrorCount}, Errors={Errors}",
                    query.templateId,
                    query.filepath,
                    fileResult.Errors.Count,
                    string.Join("; ", fileResult.Errors.Select(e => $"{e.Code}:{e.Description}")));

                return fileResult.Errors;
            }

            _logger.LogInformation(
                "File download succeeded. TemplateId={TemplateId}, Path={Path}, FileName={FileName}, Size={Size} bytes",
                query.templateId,
                query.filepath,
                fileResult.Value.FileName,
                fileResult.Value.FileStream.Length);

            return fileResult.Value;
        }
    }
}
