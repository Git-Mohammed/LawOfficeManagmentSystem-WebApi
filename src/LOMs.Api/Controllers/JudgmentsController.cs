using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LOMs.Api.Common;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Contracts.Commands.CreateContract;
using LOMs.Application.Features.Judgments.Commands;
using LOMs.Application.Features.Judgments.Queries;
using LOMs.Application.Features.Judgments.Queries.DownloadJudgmentFile;
using LOMs.Application.Features.Judgments.Queries.GetAll;
using LOMs.Application.Features.Judgments.Queries.GetAllByCaseId;
using LOMs.Contract.Requests.Judgments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace LOMs.Api.Controllers;

[ApiController]
[Route("api/cases/{caseId:guid}/judgments")]
public sealed class JudgmentsController(ICommandMediator command, IQueryMediator query) : ApiController
{
    private readonly ICommandMediator _command = command;
    private readonly IQueryMediator _query = query;

    //  GET: By ID
    [HttpGet("{judgmentId:guid}", Name = nameof(GetJudgmentById))]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Retrieves a specific judgment by ID.")]
    [EndpointDescription("Fetches judgment details for the given case and judgment ID.")]
    public async Task<IActionResult> GetJudgmentById(Guid caseId, Guid judgmentId, CancellationToken ct)
    {
        var queryRequest = new GetJudgmentByIdQuery(caseId, judgmentId);
        var result = await _query.QueryAsync(queryRequest, ct);

        return result.Match(Ok, Problem);
    }

    //  GET: Paged by CaseId
    [HttpGet("paged", Name = nameof(GetPagedJudgmentsByCaseId))]
    [ProducesResponseType(typeof(PaginatedList<JudgmentDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves paginated judgments linked to a specific case.")]
    [EndpointDescription("Fetches a paginated list of judgments associated with the given case ID.")]
    public async Task<IActionResult> GetPagedJudgmentsByCaseId(
        Guid caseId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        var result = await _query.QueryAsync(
            new GetPagedJudgmentsByCaseIdQuery(caseId, pageNumber, pageSize),
            ct);

        return result.Match(Ok, Problem);
    }

    [HttpGet("/api/judgments/paged", Name = nameof(GetPagedJudgments))]
    [ProducesResponseType(typeof(PaginatedList<JudgmentDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves all judgments in paginated form.")]
    [EndpointDescription("Fetches a paginated list of all judgments across all cases.")]
    [EndpointName(nameof(GetPagedJudgments))]
    public async Task<IActionResult> GetPagedJudgments(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    CancellationToken ct = default)
    {
        var result = await _query.QueryAsync(new GetPagedJudgmentsQuery(pageNumber, pageSize), ct);
        return result.Match(Ok, Problem);
    }
    //  GET: Download file
    [EnableRateLimiting(RateLimitPolicies.FileConcurrency)]
    [HttpGet("{judgmentId:guid}/files/download", Name = nameof(DownloadJudgmentFile))]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Downloads a specific file related to a judgment.")]
    [EndpointDescription("Downloads a file by relative path under a specific judgment and case.")]
    public async Task<IActionResult> DownloadJudgmentFile(
        Guid caseId,
        Guid judgmentId,
        [FromQuery] string relativePath,
        CancellationToken ct)
    {
        var result = await _query.QueryAsync(new DownloadJudgmentFileQuery(caseId, judgmentId, relativePath), ct);

        return result.Match(
            file => File(file.FileStream, file.ContentType, file.FileName),
            Problem
        );
    }

    //  POST: Create
    [HttpPost(Name = nameof(CreateJudgment))]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Creates a new judgment under a specific case.")]
    [EndpointDescription("Registers a new judgment for the given case and optionally uploads associated files.")]
    public async Task<IActionResult> CreateJudgment(
        Guid caseId,
        [FromForm] CreateJudgmentRequest request,
        CancellationToken ct)
    {
        await using var fileStream = request.File.OpenReadStream();

        var fileCommand = new CreateFileCommand(
            request.File.FileName,
            fileStream,
            request.File.ContentType
        );

        var commandRequest = new CreateJudgmentCommand(
            CaseId: caseId,
            Number: request.Number,
            Type: (LOMs.Domain.Cases.Judgments.JudgmentType)request.Type,
            SubType: (LOMs.Domain.Cases.Judgments.JudgmentSubType)request.SubType,
            IssuedOn: request.IssuedOn,
            FileCommand: fileCommand
        );

        var result = await _command.SendAsync(commandRequest, ct);

        return result.Match(
            id => CreatedAtRoute(
                routeName: nameof(GetJudgmentById),
                routeValues: new { caseId = caseId, judgmentId = id },
                value: new { judgmentId = id }),
            Problem
        );
    }

}
