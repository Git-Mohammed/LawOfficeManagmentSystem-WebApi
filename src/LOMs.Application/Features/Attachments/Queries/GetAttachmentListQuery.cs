using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Attachments.DTOs;
using LOMs.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Application.Features.Attachments.Queries
{
    public sealed record GetAttachmentListQuery(Guid CaseId) : IQuery<Result<List<AttachmentDto>>>;

  
}
