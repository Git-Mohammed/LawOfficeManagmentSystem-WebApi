using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Application.Features.Attachments.Queries.DownloadAttachmentFile
{
    public record class DownloadAttachmentFileQuery(Guid CaseId, string FilePath):IQuery<Result<FileDownloadResult>>;
   
}
