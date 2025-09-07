using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Cases.Commands.CreateCase;
using LOMs.Application.Features.Cases.Dtos;
using LOMs.Application.Features.Cases.Queries.GetCaseByIdQuery;
using LOMs.Application.Features.People.Clients.Commands.CreateClient;
using LOMs.Contract.Requests.Cases;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LOMs.Api.Controllers;

[Route("api/cases")]
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
            return Problem("دور العميل في القضية غير صالح.");

        if (request.Clients is null || request.Clients.Count == 0)
            return Problem("يجب ان يكون هناك عملاء");

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
            _ => throw new ValidationException("كل عنصر في قائمة العملاء يجب أن يحتوي إما على معرف موجود أو بيانات عميل جديدة.")
        }).ToList();

    #endregion
}
