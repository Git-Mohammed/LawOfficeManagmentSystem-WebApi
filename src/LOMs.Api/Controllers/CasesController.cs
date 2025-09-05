using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Cases.Commands.CreateCase;
using LOMs.Application.Features.Cases.Dtos;
using LOMs.Application.Features.Cases.Queries.GetCaseByIdQuery;
using LOMs.Application.Features.People.Clients.Commands.CreateClient;
using LOMs.Contract.Requests.Cases;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace LOMs.Api.Controllers;

[Route("api/cases")]
public class CasesController(ICommandMediator command, IQueryMediator query) : ApiController
{
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
        var result = await query.QueryAsync(queryRequest, ct);

        return result.Match(Ok, Problem);
    }

    [HttpPost(Name = nameof(CreateCase))]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Creates a new case.")]
    [EndpointDescription("Adds a new case to the system with clients, contracts, and POAs.")]
    [EndpointName(nameof(CreateCase))]
    public async Task<IActionResult> CreateCase([FromForm] CreateCaseRequest request, CancellationToken ct)
    {
        if (!Enum.IsDefined(typeof(LOMs.Contract.Commons.Enums.PartyRole), request.PartyRole))
            return Problem("دور العميل في القضية غير صالح.");

        var clients = ParseClients(request.ClientsJson);
        if (clients is null || clients.Count == 0)
            return Problem("يجب ان يكون هناك عملاء");

        var clientModels = MapClients(clients);

        var contracts = request.HasContracts
            ? ValidateAndMapContracts(request)
            : new List<CreateContractWithCaseCommand>();

        if (contracts == null) return Problem("بيانات العقود غير صالحة.");

        var poas = request.HasPOAs
            ? ValidateAndMapPOAs(request)
            : new List<CreatePOAWithCaseCommand>();

        if (poas == null) return Problem("بيانات الوكالات غير صالحة.");

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
            HasContracts: request.HasContracts,
            Contracts: contracts,
            HasPOAs: request.HasPOAs,
            POAs: poas,
            AssignedOfEmployeeId: request.AssignedEmployeeId
        );

        var result = await command.SendAsync(createCaseCommand, ct);

        return result.Match(
            success => CreatedAtRoute(
                routeName: nameof(GetCaseById),
                routeValues: new { caseId = success.Id },
                value: new { Id = success.Id }),
            Problem);
    }

    #region Helpers

    private static List<CaseClientRequest>? ParseClients(string? clientsJson)
    {
        if (string.IsNullOrWhiteSpace(clientsJson))
            return null;

        try
        {
            return JsonSerializer.Deserialize<List<CaseClientRequest>>(clientsJson);
        }
        catch
        {
            return null;
        }
    }

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

    private static List<CreateContractWithCaseCommand>? ValidateAndMapContracts(CreateCaseRequest request)
    {
        if (request.ContractsData is null || request.ContractsData.Count == 0)
            return null;

        if (request.ContractFiles is null || request.ContractFiles.Count != request.ContractsData.Count)
            return null;

        var contracts = new List<CreateContractWithCaseCommand>();

        for (int i = 0; i < request.ContractsData.Count; i++)
        {
            var data = request.ContractsData[i];
            var file = request.ContractFiles[i];

            if (!Enum.IsDefined(typeof(LOMs.Contract.Commons.Enums.ContractType), (LOMs.Contract.Commons.Enums.ContractType)data.ContractType))
                return null;

            if (file.Length == 0)
                return null;

            contracts.Add(new CreateContractWithCaseCommand(
                (LOMs.Domain.Cases.Enums.ContractType)data.ContractType,
                data.IssueDate,
                data.ExpiryDate,
                data.BaseAmount,
                data.InitialPayment,
                file.OpenReadStream(),
                file.FileName,
                data.IsAssigned
            ));
        }

        return contracts;
    }

    private static List<CreatePOAWithCaseCommand>? ValidateAndMapPOAs(CreateCaseRequest request)
    {
        if (request.POAsData is null || request.POAFiles is null || request.POAFiles.Count != request.POAsData.Count)
            return null;

        var poas = new List<CreatePOAWithCaseCommand>();

        for (int i = 0; i < request.POAsData.Count; i++)
        {
            var data = request.POAsData[i];
            var file = request.POAFiles[i];

            if (string.IsNullOrWhiteSpace(data.POANumber) ||
                string.IsNullOrWhiteSpace(data.IssuingAuthority) ||
                data.IssueDate > DateOnly.FromDateTime(DateTime.UtcNow) ||
                file == null || file.Length == 0)
            {
                return null;
            }

            poas.Add(new CreatePOAWithCaseCommand(
                data.POANumber.Trim(),
                data.IssueDate,
                data.IssuingAuthority.Trim(),
                file.OpenReadStream(),
                file.FileName
            ));
        }

        return poas;
    }

    #endregion
}
