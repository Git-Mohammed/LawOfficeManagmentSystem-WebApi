using LiteBus.Commands.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Domain.Common.Files;
using LOMs.Domain.Common.Results;
using LOMs.Domain.Templates;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LOMs.Application.Features.Templates.Commands
{
    public sealed class CreateTemplateCommandHandler:ICommandHandler<CreateTemplateCommand,Result<Guid>>
    {
        private readonly IAppDbContext _context;
        private readonly ILogger<CreateTemplateCommandHandler> _logger;
        private readonly IFileService _fileService;
        public CreateTemplateCommandHandler(
            IAppDbContext context,
            ILogger<CreateTemplateCommandHandler> logger,
            IFileService fileService)
         {
            _context = context;
            _logger = logger;
            _fileService = fileService;

         }

        public async Task<Result<Guid>> HandleAsync(CreateTemplateCommand command, CancellationToken cancellationToken = default)
        {
            var savedFile = await _fileService.SaveFileAsync(
                 command.FileCommand.File, command.FileCommand.FileName, nameof(FileType.Template));
            if (savedFile.IsError)
                return savedFile.Errors;


            var template = Template.Create(
               id: Guid.NewGuid(),

                templatename: command.TemplateName,
                filename: command.FileCommand.FileName,
                filepath: savedFile.Value,
                 fileType: FileType.Template,
               size: command.FileCommand.File.Length,
            extenstion: Path.GetExtension(command.FileCommand.FileName),
            contentType: command.FileCommand.ContentType
                );

            if (template.IsError)
                return template.Errors;

            await _context.Templates.AddAsync(template.Value, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return template.Value.Id;
        }
    }
}
