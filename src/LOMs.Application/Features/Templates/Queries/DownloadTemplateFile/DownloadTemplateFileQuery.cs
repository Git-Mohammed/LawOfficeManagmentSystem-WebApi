using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Application.Features.Templates.Queries.DownloadTemplateFile
{
    public sealed  record DownloadTemplateFileQuery(Guid templateId, string filepath) :IQuery<Result<FileDownloadResult>>;
  
}
