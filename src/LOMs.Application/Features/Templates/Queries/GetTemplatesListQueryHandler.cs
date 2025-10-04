using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Tamplates.DTOs;
using LOMs.Application.Features.Templates.DTOs;
using LOMs.Domain.Common.Results;
using LOMs.Domain.Templates;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Application.Features.Templates.Queries
{
    public sealed class GetTemplatesListQueryHandler(
        IAppDbContext Context,
        ILogger<GetTemplatesListQueryHandler> logger
        ) : IQueryHandler<GetTemplatesListQuery, Result<List<TemplateDto>>>
    {
        public async Task<Result<List<TemplateDto>>> HandleAsync(GetTemplatesListQuery query, CancellationToken ct = default)
        {
            var templates = await Context.Templates
        .AsNoTracking()
          .Select(a => new TemplateDto
          {
              TemplateId = a.Id,
              TemplateName = a.TemplateName,
              File = new FileDto
              {
                  FilePath = a.Filepath ?? string.Empty,      
                  FileName = a.Filename ?? string.Empty,      
                  FileType = a.ContentType ?? string.Empty   
              }

          })
          .ToListAsync(ct);

            if (templates.Count == 0)
            {
                logger.LogWarning("templates not found.");
                return TemplateErrors.NotFound;
            }


            return templates;

        }
    }
}
