using LiteBus.Commands.Abstractions;
using LOMs.Application.Features.Contracts.Commands.CreateContract;
using LOMs.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Application.Features.Templates.Commands
{
    public sealed record class CreateTemplateCommand(
      string TemplateName,
     CreateFileCommand FileCommand) :ICommand<Result<Guid>>;
   
}
