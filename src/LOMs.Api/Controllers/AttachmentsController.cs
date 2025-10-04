using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LOMs.Api.Common;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Attachments.Commands;
using LOMs.Application.Features.Attachments.DTOs;
using LOMs.Application.Features.Attachments.Queries.DownloadAttachmentFile;
using LOMs.Application.Features.Attachments.Queries.GetAttachmentsByCaseId.Paged;
using LOMs.Application.Features.Contracts.Commands.CreateContract;
using LOMs.Contract.Requests.Attachments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace LOMs.Api.Controllers;

[Route("api/cases/{caseId:guid}/attachments")]
[Authorize]
public class AttachmentsController(ICommandMediator command, IQueryMediator query) : ApiController
{
    private readonly ICommandMediator _command = command;
    private readonly IQueryMediator _query = query;

    [HttpGet(Name = nameof(GetAttachmentsByCaseId))]
    [ProducesResponseType(typeof(PaginatedList<AttachmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Get all attachments for a given case")]
    [EndpointDescription("Retrieves a paginated list of attachments linked to the specified case.")]
    [EndpointName(nameof(GetAttachmentsByCaseId))]
    public async Task<IActionResult> GetAttachmentsByCaseId(
        Guid caseId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var queryRequest = new GetPagedAttachmentsByCaseIdQuery(caseId, pageNumber, pageSize);
        var result = await _query.QueryAsync(queryRequest, ct);
        return result.Match(Ok, Problem);
    }

    [EnableRateLimiting(RateLimitPolicies.FileConcurrency)]
    [HttpGet("{attachmentId:guid}/files/download", Name = nameof(DownloadAttachmentFile))]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Downloads a specific file related to a Case.")]
    [EndpointDescription("Downloads a file by relative path under a specific case.")]
    [EndpointName(nameof(DownloadAttachmentFile))]
    public async Task<IActionResult> DownloadAttachmentFile(
        Guid caseId,
        Guid attachmentId,
        [FromQuery] string filePath,
        CancellationToken ct)
    {
        var result = await _query.QueryAsync(new DownloadAttachmentFileQuery(caseId, attachmentId, filePath), ct);
        return result.Match(
            file => File(file.FileStream, file.ContentType, file.FileName),
            Problem
        );
    }

    [HttpPost(Name = nameof(CreateAttachment))]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Create New Attachment under Specific Case")]
    [EndpointDescription("Registers a new Attachment for the given case and optionally uploads associated files.")]
    [EndpointName(nameof(CreateAttachment))]
    public async Task<IActionResult> CreateAttachment(
        Guid caseId,
        [FromForm] CreateAttachmentRequest request,
        CancellationToken ct)
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
            AttachmentFile: fileCommand
        );

        var result = await _command.SendAsync(commandRequest, ct);
        return result.Match(id => CreatedAtRoute(nameof(GetAttachmentsByCaseId),new {caseId = caseId} ,new { attachmentId = id }), Problem);
    }
}
