using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Contracts.Commands.CreateContract;
using LOMs.Application.Features.Judgments.Commands;
using LOMs.Application.Features.Judgments.Queries;
using LOMs.Contract.Requests.Judgments;
using Microsoft.AspNetCore.Mvc;

namespace LOMs.Api.Controllers;

[Route("api/cases/{caseId:guid}/judgments")]
public class JudgmentsController(ICommandMediator command, IQueryMediator query) : ApiController
{
    private readonly ICommandMediator _command = command;
    private readonly IQueryMediator _query = query;
    
    
    [HttpPost(Name = nameof(CreateJudgment))]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Creates a new judgment under a specific case.")]
    [EndpointDescription("Registers a new judgment for the given case and optionally uploads associated files.")]
    [EndpointName(nameof(CreateJudgment))]
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
            
        // Map the request DTO into a command
        var commandRequest = new CreateJudgmentCommand(
            CaseId: caseId,
            Number: request.Number,
            Type: request.Type,
            SubType: request.SubType,
            IssuedOn: request.IssuedOn,
            ExpiresOn: request.ExpiresOn,
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

    // GET by id (to support CreatedAtRoute)
    [HttpGet("{judgmentId:guid}", Name = nameof(GetJudgmentById))]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJudgmentById(Guid caseId, Guid judgmentId, CancellationToken ct)
    {
        // Placeholder query (to be implemented later)
        var query = new GetJudgmentByIdQuery(caseId, judgmentId);
        var result = await _query.QueryAsync(query, ct);

        return result.Match<IActionResult>(
            response => Ok(response),
            Problem
        );
    }
    
}