using LiteBus.Commands.Abstractions;
using LOMs.Application.Features.Attachments.DTOs;
using LOMs.Application.Features.Contracts.Commands.CreateContract;
using LOMs.Domain.Common.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Application.Features.Attachments.Commands
{
    public sealed record CreateAttachmentCommand(
     Guid CaseId,
     string AttachmentName,
     CreateFileCommand AttachmentFile
    
 ) : ICommand<Result<Guid>>;
}
