using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Cases.Commands.CreateCase;
using LOMs.Application.Features.Cases.Commands.UpdateStatus;
using LOMs.Application.Features.Cases.Dtos;
using LOMs.Application.Features.Cases.Queries.GetCaseByIdQuery;
using LOMs.Application.Features.Cases.Queries.GetCaseQuickStatsByCourtTypeAndHijriYear;
using LOMs.Application.Features.People.Clients.Commands.CreateClient;
using LOMs.Contract.Requests.Cases;
using LOMs.Domain.Cases.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LOMs.Api.Controllers;

[Route("api/cases")]
[Authorize]
public class CasesController(ICommandMediator command, IQueryMediator query) : ApiController
{
    private readonly IQueryMediator _query = query;
    private readonly ICommandMediator _command = command;

    [HttpGet("{caseId:guid}", Name = nameof(GetCaseById))]
    [ProducesResponseType(typeof(CaseDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves case details by ID.")]
    [EndpointDescription("Fetches a case and its associated data using the case identifier.")]
    [EndpointName(nameof(GetCaseById))]
    public async Task<IActionResult> GetCaseById(Guid caseId, CancellationToken ct)
    {
        var queryRequest = new GetCaseByIdQuery(caseId);
        var result = await _query.QueryAsync(queryRequest, ct);

        return result.Match(Ok, Problem);
    }
[HttpGet("by-court-type/{courtTypeId:guid}/year/{hijriYear:int}", Name = nameof(GetCasesByCourtTypeAndYearPaginated))]
[ProducesResponseType(typeof(PaginatedList<CaseQuickStatsDto>), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
[EndpointSummary("Retrieves paginated case statistics by court type and Hijri year.")]
[EndpointDescription("Fetches a paginated list of case quick statistics filtered by court type, Hijri year, and optionally by status.")]
[EndpointName(nameof(GetCasesByCourtTypeAndYearPaginated))]
public async Task<IActionResult> GetCasesByCourtTypeAndYearPaginated(
    Guid courtTypeId,
    int hijriYear,
    [FromQuery] CaseStatus? status,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    CancellationToken ct = default)
{
        if (pageNumber <= 0)
            return Problem("Page number must be greater than zero.");

        if (pageSize <= 0)
            return Problem("Page size must be greater than zero.");

        var queryRequest = new GetCaseQuickStatsByCourtTypeAndHijriYearQuery(
        CourtTypeId: courtTypeId,
        HijriYear: (short)hijriYear,
        PageNumber: pageNumber,
        PageSize: pageSize,
        Status: status
    );

    var result = await _query.QueryAsync(queryRequest, ct);

    return result.Match(Ok, Problem);
}


    [HttpPost(Name = nameof(CreateCase))]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Creates a new case.")]
    [EndpointDescription("Adds a new case to the system with clients, contracts, and POAs.")]
    [EndpointName(nameof(CreateCase))]
    public async Task<IActionResult> CreateCase([FromBody] CreateCaseRequest request, CancellationToken ct)
    {
        if (!Enum.IsDefined(typeof(LOMs.Contract.Commons.Enums.PartyRole), request.PartyRole))
            return Problem("The client's role in the case is invalid.");

        if (request.Clients is null || request.Clients.Count == 0)
            return Problem("At least one client must be provided.");

        var clientModels = MapClients(request.Clients);


        var createCaseCommand = new CreateCaseCommand(
            Clients: clientModels,
            CaseNumber: request.CaseNumber,
            CaseSubject: request.CaseSubject,
            CourtTypeId: request.CourtTypeId,
            PartyRole: (LOMs.Domain.Cases.Enums.PartyRole)request.PartyRole,
            ClientRequests: request.ClientRequestDetails,
            EstimatedReviewDate: request.EstimatedReviewDate,
            LawyerOpinion: request.LawyerOpinion,
            IsDraft: request.IsDraft,
            AssignedOfEmployeeId: request.AssignedEmployeeId
        );

        var result = await _command.SendAsync(createCaseCommand, ct);

        return result.Match(
            Id => CreatedAtRoute(
                routeName: nameof(GetCaseById),
                routeValues: new { caseId = Id },
                value: new { Id = Id }),
            Problem);
    }


    [HttpPut("{caseId:guid}/status", Name = nameof(UpdateCaseStatus))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Update case status")]
    [EndpointDescription("Used to update the status of an existing case using its ID and the new status.")]
    [EndpointName(nameof(UpdateCaseStatus))]
    public async Task<IActionResult> UpdateCaseStatus(
        Guid caseId,
        [FromBody] UpdateCaseStatusRequest request,
        CancellationToken ct = default)
    {
        if (!Enum.IsDefined(typeof(LOMs.Contract.Commons.Enums.CaseStatus), request.Status))
            return Problem("The provided status value is invalid.");

        var result = await _command.SendAsync(
            new UpdateCaseStatusCommand(caseId, (LOMs.Domain.Cases.Enums.CaseStatus)request.Status),
            ct);

        return result.Match(_ => NoContent(), Problem);
    }


    #region Helpers
    private static List<CaseClientModel> MapClients(List<CaseClientRequest> clients) =>
        clients.Select<CaseClientRequest, CaseClientModel>(c => c switch
        {
            ExistingClientRequest existing => new ExistingCaseClientModel
            {
                ExistingClientId = existing.ClientId
            },
            NewClientRequest newClient => new NewCaseClientModel(
                new CreateClientCommand(
                    new CreatePersonCommand(
                        newClient.Client.Person.FullName,
                        newClient.Client.Person.NationalId,
                        newClient.Client.Person.CountryCode,
                        newClient.Client.Person.BirthDate,
                        newClient.Client.Person.PhoneNumber,
                        newClient.Client.Person.Address
                    )
                )
            ),
            _ => throw new ValidationException("Each item in the client list must contain either an existing client ID or new client data.")
        }).ToList();

    #endregion
}
