using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LOMs.Api.Common;
using LOMs.Application.Features.Attachments.Queries.DownloadAttachmentFile;
using LOMs.Application.Features.Contracts.Commands.CreateContract;
using LOMs.Application.Features.Tamplates.DTOs;
using LOMs.Application.Features.Templates.Commands;
using LOMs.Application.Features.Templates.Queries;
using LOMs.Application.Features.Templates.Queries.DownloadTemplateFile;
using LOMs.Contract.Requests.Templates;
using LOMs.Domain.Templates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace LOMs.Api.Controllers
{
    [Route("api/templates")]
    
    public class TemplatesController(ICommandMediator command , IQueryMediator query) :ApiController
    {
        private readonly ICommandMediator _command = command;
        private readonly IQueryMediator _query = query;

        [HttpPost(Name = nameof(CreateTemplate))]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Creates a new template .")]
        [EndpointDescription("Registers a new general template .")]
        [EndpointName(nameof(CreateTemplate))]
        public async Task<IActionResult> CreateTemplate(
        [FromForm] CreateTemplateRequest request,
       CancellationToken ct)
        {

            await using var fileStream = request.File.OpenReadStream();

            var fileCommand = new CreateFileCommand(
                request.File.FileName,
                fileStream,
                request.File.ContentType
            );

            // Map the request DTO into a command
            var commandRequest = new CreateTemplateCommand(
               
                TemplateName : request.TemplateName,
                FileCommand: fileCommand
            );


            var result = await _command.SendAsync(commandRequest, ct);
            return result.Match<IActionResult>(
            response => Ok(response),
            Problem);
        }

        [HttpGet(Name = nameof(GetAllTemplates))]
        [ProducesResponseType(typeof(List<TemplateDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Gets all templates.")]
        [EndpointDescription("Retrieves all general templates.")]
        [EndpointName(nameof(GetAllTemplates))]
        public async Task<IActionResult> GetAllTemplates(CancellationToken ct)
        {
            var queryRequest = new GetTemplatesListQuery();

            var result = await _query.QueryAsync(queryRequest, ct);

            
            return result.Match<IActionResult>(
                templates => Ok(templates),
                Problem
            );
        }

        [EnableRateLimiting(RateLimitPolicies.FileConcurrency)]
        [HttpGet("{templateId:guid}/files/download", Name = nameof(DownloadTemplateFile))]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [EndpointSummary("Downloads a specific file related to a templateId.")]
        [EndpointDescription("Downloads a file by relative path under a specific template.")]
        [EndpointName(nameof(DownloadTemplateFile))]
        public async Task<IActionResult> DownloadTemplateFile(
       Guid templateId,
       [FromQuery] string filePath,
       CancellationToken ct)
        {
            var result = await _query.QueryAsync(new DownloadTemplateFileQuery(templateId, filePath), ct);
            return result.Match(
                file => File(file.FileStream, file.ContentType, file.FileName),
                Problem
            );
        }
    }
}
