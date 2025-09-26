using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Attachments.Commands;
using LOMs.Application.Features.Attachments.DTOs;
using LOMs.Application.Features.Attachments.Queries;
using LOMs.Application.Features.Contracts.Commands.CreateContract;
using LOMs.Application.Features.Judgments.Queries;
using LOMs.Contract.Requests.Attachments;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LOMs.Api.Controllers;

    [Route("api/cases/attachments")]
    public class AttachmentsController(ICommandMediator command, IQueryMediator query) : ApiController
    {
        private readonly ICommandMediator _command = command;
        private readonly IQueryMediator _query = query;

        [HttpPost(Name = nameof(CreateAttachment))]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Create New Attachment under Specific Case")]
        [EndpointDescription("Registers a new Attachment for the given case and optionally uploads associated files.")]
        [EndpointName(nameof(CreateAttachment))]
        public async Task<IActionResult> CreateAttachment(
            Guid caseId,
            [FromForm] CreateAttachmentRequest request, CancellationToken ct)
        {

            await using var fileStream = request.File.OpenReadStream();

            var fileCommand = new CreateFileCommand(
                request.File.FileName,
                fileStream,
                request.File.ContentType
            );

            var commandRequest = new CreateAttachmentCommand(
               CaseId: caseId,
               AttachmentName: request.AttachmentName,
               AttachmentFile: fileCommand);

            var result = await _command.SendAsync(commandRequest, ct);
            return result.Match<IActionResult>(
            response => Ok(response),
            Problem
         );
        }
        [HttpGet("{caseId:guid}",Name = nameof(GetAttachmentsByCaseId))]
        [ProducesResponseType(typeof(List<AttachmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Get all attachments for a given case")]
        [EndpointDescription("Retrieves the list of attachments linked to the specified case.")]
        [EndpointName(nameof(GetAttachmentsByCaseId))]
        public async Task<IActionResult> GetAttachmentsByCaseId( Guid caseId  , CancellationToken tc)
        {
            var queryRequest = new GetAttachmentListQuery(caseId);
            
            var result = await _query.QueryAsync(queryRequest, tc);
            return result.Match<IActionResult>(
                response => Ok(response),
                Problem);
        }

    }

