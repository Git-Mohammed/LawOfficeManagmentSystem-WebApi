using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Contracts.Commands.CreateContract;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Application.Features.Contracts.Queries;
using LOMs.Application.Features.Contracts.Queries.GetContractById;
using LOMs.Application.Features.Contracts.Queries.GetContractList;
using LOMs.Application.Features.Contracts.Queries.GetContractList.Paged;
using LOMs.Application.Features.Contracts.Queries.GetContractsDetailsByCaseId.Paged;
using LOMs.Contract.Requests.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LOMs.Api.Controllers;

[Route("api/cases/{caseId:guid}/contracts")]
[Authorize]
public class ContractsController(ICommandMediator command, IQueryMediator query) : ApiController
{
    private readonly ICommandMediator _command = command;
    private readonly IQueryMediator _query = query;



    [HttpGet("{contractId:guid}", Name = nameof(GetContractById))]
    [ProducesResponseType(typeof(ContractDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves contract details by ID under a specific case.")]
    [EndpointDescription("Fetches a contract by its ID for the given case.")]
    [EndpointName(nameof(GetContractById))]
    public async Task<IActionResult> GetContractById(Guid caseId, Guid contractId, CancellationToken ct)
    {
        var result = await _query.QueryAsync(new GetContractByIdQuery(caseId,contractId), ct);
        return result.Match(Ok, Problem);
    }

    [HttpGet(Name = nameof(GetContractsByCaseId))]
    [ProducesResponseType(typeof(List<ContractDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves all contracts linked to a specific case.")]
    [EndpointDescription("Fetches a list of contracts associated with the given case ID.")]
    [EndpointName(nameof(GetContractsByCaseId))]
    public async Task<IActionResult> GetContractsByCaseId(Guid caseId, CancellationToken ct)
    {
        var result = await _query.QueryAsync(new GetContractsDetailsByCaseIdQuery(caseId), ct);
        return result.Match(Ok, Problem);
    }


    [HttpGet("paged", Name = nameof(GetPagedContractsByCaseId))]
    [ProducesResponseType(typeof(PaginatedList<ContractDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves paginated contracts linked to a specific case.")]
    [EndpointDescription("Fetches a paginated list of contracts associated with the given case ID.")]
    [EndpointName(nameof(GetPagedContractsByCaseId))]
    public async Task<IActionResult> GetPagedContractsByCaseId(
    Guid caseId,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    CancellationToken ct = default)
    {
        var result = await _query.QueryAsync(
            new GetPagedContractsByCaseIdQuery(caseId, pageNumber, pageSize),
            ct);

        return result.Match(Ok, Problem);
    }

    [HttpGet("/api/contracts/paged", Name = nameof(GetPagedContractList))]
    [ProducesResponseType(typeof(PaginatedList<ContractListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves a paginated list of contracts.")]
    [EndpointDescription("Returns contract number and type for each contract, with pagination.")]
    [EndpointName(nameof(GetPagedContractList))]
    public async Task<IActionResult> GetPagedContractList(
    [FromQuery] int pageIndex = 1,
    [FromQuery] int pageSize = 10,
    CancellationToken ct = default)
    {
        var result = await _query.QueryAsync(
            new GetPagedContractListQuery(pageIndex, pageSize),
            ct);

        return result.Match(Ok, Problem);
    }

    [HttpGet("/api/contracts", Name = nameof(GetContractList))]
    [ProducesResponseType(typeof(List<ContractListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves a list of contracts.")]
    [EndpointDescription("Returns contract number and type for each contract.")]
    [EndpointName(nameof(GetContractList))]
    public async Task<IActionResult> GetContractList(CancellationToken ct)
    {
        var result = await _query.QueryAsync(new GetContractListQuery(), ct);
        return result.Match(Ok, Problem);
    }

    [HttpPost(Name = nameof(CreateContract))]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Creates a new contract under a specific case.")]
    [EndpointDescription("Registers a new contract for the given case and uploads associated files.")]
    [EndpointName(nameof(CreateContract))]
    public async Task<IActionResult> CreateContract(
        Guid caseId,
        [FromForm] CreateContractRequest request,
        CancellationToken ct)
    {
        await using var fileStream = request.ContractFile.OpenReadStream();

        var fileCommand = new CreateFileCommand(
            request.ContractFile.FileName, 
            fileStream,
            request.ContractFile.ContentType
        );

        var commandRequest = new CreateContractCommand(
            CaseId: caseId,
            ContractType: (Domain.Cases.Enums.ContractType)request.ContractType,
            IssuedOn: request.IssuedOn,
            ExpiresOn: request.ExpiresOn,
            TotalAmount: request.BaseAmount,
            InitialPayment: request.InitialPayment,
            IsAssigned: request.IsAssigned,
            ContractFile: fileCommand
        );

        var result = await _command.SendAsync(commandRequest, ct);

        return result.Match(
            id => CreatedAtRoute(
                routeName: nameof(GetContractById),
                routeValues: new { caseId = caseId, contractId = id },
                value: new { contractId = id }),
            Problem
        );
    }

}
