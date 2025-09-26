using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Attachments.DTOs;
using LOMs.Domain.Cases.Attachments;
using LOMs.Domain.Cases.Judgments;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LOMs.Application.Features.Attachments.Queries
{
    public sealed class GetAttachmentListQueryHandler(
        IAppDbContext Context,
        ILogger<GetAttachmentListQueryHandler> logger 
        )
         : IQueryHandler<GetAttachmentListQuery, Result<List<AttachmentDto>>>
    {


        public async Task<Result<List<AttachmentDto>>> HandleAsync(GetAttachmentListQuery message, CancellationToken ct = default)
        {

            var attachments = await Context.Attachments
        .AsNoTracking()
          .Where(a => a.CaseId == message.CaseId)
          .Select(a => new AttachmentDto
          {
              AttachmentName = a.AttachmentName,
              CreatedDate = a.CreatedAtUtc,
              CreatedByEmployeeName = a.CreatedBy
          })
          .ToListAsync(ct);

            if (attachments.Count == 0)
            {
                logger.LogWarning("attchment with Id {caseId} not found.", message.CaseId);
                return AttachmentErrors.NotFound;
            }


            return attachments;


        }
    }
}
