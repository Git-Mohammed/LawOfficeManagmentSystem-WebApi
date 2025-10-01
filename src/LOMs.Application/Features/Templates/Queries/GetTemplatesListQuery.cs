using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Tamplates.DTOs;
using LOMs.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Application.Features.Templates.Queries
{
   public sealed record  GetTemplatesListQuery() : IQuery<Result<List<TemplateDto>>>;

}
