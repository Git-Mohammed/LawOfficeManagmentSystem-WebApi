using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Tamplates.DTOs;
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
              TemplateName = a.TemplateName,
              FilePath = a.Filepath
            
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
