using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.POAs.Commands.CreatePOA;
using LOMs.Application.Features.POAs.Dtos;
using LOMs.Application.Features.POAs.Queries.GetById;
using LOMs.Contract.Requests.POAs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LOMs.Api.Controllers;

[Route("api/cases/{caseId:guid}/poas")]
[Authorize]
public class POAsController(ICommandMediator command, IQueryMediator query) : ApiController
{
    private readonly ICommandMediator _command = command;
    private readonly IQueryMediator _query = query;

    [HttpGet("{poaId:guid}", Name = nameof(GetPOAById))]
    [ProducesResponseType(typeof(POADto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves POA details by ID under a specific case.")]
    [EndpointDescription("Fetches a POA by its ID for the given case.")]
    [EndpointName(nameof(GetPOAById))]
    public async Task<IActionResult> GetPOAById(Guid caseId, Guid poaId, CancellationToken ct)
    {
        var result = await _query.QueryAsync(new GetPOAByIdQuery(caseId, poaId), ct);
        return result.Match(Ok, Problem);
    }


    [HttpPost(Name = nameof(CreatePOA))]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Creates a new POA under a specific case.")]
    [EndpointDescription("Registers a new POA for the given case and uploads the associated file.")]
    [EndpointName(nameof(CreatePOA))]
    public async Task<IActionResult> CreatePOA(
        Guid caseId,
        [FromForm] CreatePOARequest request,
        CancellationToken ct)
    {
        await using var fileStream = request.AttachmentFile.OpenReadStream();

        var fileCommand = new CreatePOAFileWithPOACommand(
            request.AttachmentFile.FileName,
            fileStream,
            request.AttachmentFile.ContentType
        );

        var commandRequest = new CreatePOACommand(
            CaseId: caseId,
            POANumber: request.POANumber,
            IssueDate: request.IssueDate!.Value,
            IssuingAuthority: request.IssuingAuthority,
            POAFile: fileCommand
        );

        var result = await _command.SendAsync(commandRequest, ct);

        return result.Match(
            id => CreatedAtRoute(
                routeName: nameof(GetPOAById),
                routeValues: new { caseId = caseId, poaId = id },
                value: new { poaId = id }),
            Problem
        );
    }
}
