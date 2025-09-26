using LiteBus.Commands.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Attachments.DTOs;
using LOMs.Domain.Cases;
using LOMs.Domain.Cases.Attachments;
using LOMs.Domain.Common.Files;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LOMs.Application.Features.Attachments.Commands
{
    public  class CreateAttachmentCommandHandler : ICommandHandler<CreateAttachmentCommand, Result<Guid>>
    {
        private readonly IAppDbContext context;
        private readonly ILogger<CreateAttachmentCommandHandler> logger;
        private readonly IFileService fileService;

        public CreateAttachmentCommandHandler(
          IAppDbContext context,
          ILogger<CreateAttachmentCommandHandler> logger,
          IFileService fileService)
        {
            this.context = context;
            this.logger = logger;
            this.fileService = fileService;
        }

        public async Task<Result<Guid>> HandleAsync(CreateAttachmentCommand command, CancellationToken cancellationToken = new CancellationToken())
        {
            var isExistingCaseEntity = await context.Cases.AnyAsync(c => c.Id == command.CaseId, cancellationToken);

            if (!isExistingCaseEntity)
            {
                logger.LogWarning("Case with Id {CaseId} not found.", command.CaseId);
                return CaseErrors.CaseNotFound;
            }

            var savedFile = await fileService.SaveFileAsync(
                command.AttachmentFile.File, command.AttachmentFile.FileName, nameof(FileType.Attachment));
            if (savedFile.IsError)
                return savedFile.Errors;

            var attachment = Attachment.Create(
               id: Guid.NewGuid(),
               caseId: command.CaseId,
                attachmentName: command.AttachmentName,
                filename: command.AttachmentFile.FileName,
                filepath: savedFile.Value,
                 fileType: FileType.Attachment,
               size: command.AttachmentFile.File.Length,
            extenstion: Path.GetExtension(command.AttachmentFile.FileName),
            contentType: command.AttachmentFile.ContentType
                );

            if(attachment.IsError)
                return attachment.Errors;

            await context.Attachments.AddAsync(attachment.Value, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return  attachment.Value.Id;

        }
    }
}
