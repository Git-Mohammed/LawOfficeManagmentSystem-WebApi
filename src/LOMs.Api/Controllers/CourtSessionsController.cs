using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.CourtSessions.Commands;
using LOMs.Application.Features.CourtSessions.Commands.UpdateCourtSession;
using LOMs.Application.Features.CourtSessions.Dtos;
using LOMs.Application.Features.CourtSessions.Queries.GetCourtSessionById;
using LOMs.Application.Features.CourtSessions.Queries.GetCourtSessionsByCaseId.Paged;
using LOMs.Application.Features.CourtSessions.Queries.GetFutureCourtSessions;
using LOMs.Application.Features.CourtSessions.Queries.GetFutureCourtSessions.Paged;
using LOMs.Contract.Requests.CourtSessions;
using Microsoft.AspNetCore.Mvc;

namespace LOMs.Api.Controllers;

[Route("api/cases/{caseId:guid}/court-sessions")]
public class CourtSessionsController(ICommandMediator command, IQueryMediator query) : ApiController
{
    private readonly ICommandMediator _command = command;
    private readonly IQueryMediator _query = query;


    [HttpGet("{sessionId:guid}", Name = nameof(GetCourtSessionById))]
    [ProducesResponseType(typeof(CourtSessionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves court session details by ID under a specific case.")]
    [EndpointDescription("Fetches a court session by its ID for the given case.")]
    [EndpointName(nameof(GetCourtSessionById))]
    public async Task<IActionResult> GetCourtSessionById(Guid caseId, Guid sessionId, CancellationToken ct)
    {
        var result = await _query.QueryAsync(new GetCourtSessionByIdQuery(caseId, sessionId), ct);
        return result.Match(Ok, Problem);
    }

    [HttpGet("/api/court-sessions/future", Name = nameof(GetFutureCourtSessions))]
    [ProducesResponseType(typeof(IReadOnlyList<CourtSessionDto>), StatusCodes.Status200OK)]
    [EndpointSummary("Retrieves all court sessions scheduled in the future.")]
    [EndpointDescription("Returns a list of court sessions with scheduled dates after now.")]
    [EndpointName(nameof(GetFutureCourtSessions))]
    public async Task<IActionResult> GetFutureCourtSessions(CancellationToken ct)
    {
        var query = new GetFutureCourtSessionsQuery();
        var result = await _query.QueryAsync(query, ct);
        return result.Match(Ok, Problem);
    }

    [HttpGet("/api/court-sessions/future/paged", Name = nameof(GetPaginatedFutureCourtSessions))]
    [ProducesResponseType(typeof(PaginatedList<CourtSessionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves paginated court sessions scheduled in the future.")]
    [EndpointDescription("Returns a paginated list of court sessions with scheduled dates after now.")]
    [EndpointName(nameof(GetPaginatedFutureCourtSessions))]
    public async Task<IActionResult> GetPaginatedFutureCourtSessions(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 20,
    CancellationToken ct = default)
    {
        var query = new GetPaginatedFutureCourtSessionsQuery(pageNumber, pageSize);
        var result = await _query.QueryAsync(query, ct);
        return result.Match(Ok, Problem);
    }

    [HttpGet("paged", Name = nameof(GetPaginatedCourtSessionsByCaseId))]
    [ProducesResponseType(typeof(PaginatedList<CourtSessionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves paginated court sessions under a specific case.")]
    [EndpointDescription("Returns a paginated list of court sessions for the given case ID. Optionally filter by scheduled date.")]
    [EndpointName(nameof(GetPaginatedCourtSessionsByCaseId))]
    public async Task<IActionResult> GetPaginatedCourtSessionsByCaseId(
        Guid caseId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] DateTimeOffset? fromDate = null,
        CancellationToken ct = default)
    {
        var query = new GetPaginatedCourtSessionsByCaseIdQuery(caseId, pageNumber, pageSize, fromDate);
        var result = await _query.QueryAsync(query, ct);
        return result.Match(Ok, Problem);
    }



    [HttpPost(Name = nameof(CreateCourtSession))]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Creates a new court session under a specific case.")]
    [EndpointDescription("Registers a new court session for the given case.")]
    [EndpointName(nameof(CreateCourtSession))]
    public async Task<IActionResult> CreateCourtSession(
        Guid caseId,
        [FromBody] CreateCourtSessionRequest request,
        CancellationToken ct)
    {
        var command = new CreateCourtSessionCommand
        (
            caseId,
            request.AssignedEmployeeId,
           request.ScheduledAt,
             request.AssignedTasks
        );

        var result = await _command.SendAsync(command, ct);

        return result.Match(
            id => CreatedAtRoute(
                routeName: nameof(GetCourtSessionById),
                routeValues: new { caseId = caseId, sessionId = id },
                value: new { sessionId = id }),
            Problem
        );
    }
    [HttpPut("{sessionId:guid}", Name = nameof(UpdateCourtSession))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Updates an existing court session under a specific case.")]
    [EndpointDescription("Modifies the scheduled time, tasks, or assigned employee for a court session.")]
    [EndpointName(nameof(UpdateCourtSession))]
    public async Task<IActionResult> UpdateCourtSession(
    Guid caseId,
    Guid sessionId,
    [FromBody] UpdateCourtSessionRequest request,
    CancellationToken ct)
    {
        var command = new UpdateCourtSessionCommand(
            CaseId: caseId,
            SessionId: sessionId,
            NewDate: request.NewDate,
            AssignedTasks: request.AssignedTasks,
            AssignedEmployeeId: request.AssignedEmployeeId
        );

        var result = await _command.SendAsync(command, ct);

        return result.Match(
            _ => NoContent(),
            Problem
        );
    }

}
