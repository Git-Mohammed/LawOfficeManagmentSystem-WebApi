using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Contracts.Commands.CreateContract;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Application.Features.Contracts.Queries.GetContractById;
using LOMs.Contract.Requests.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LOMs.Api.Controllers;

[Route("api/cases/{caseId:guid}/contracts")]
public class ContractsController(ICommandMediator command, IQueryMediator query) : ApiController
{
    private readonly ICommandMediator _command = command;
    private readonly IQueryMediator _query = query;



    [HttpGet("{contractId:guid}", Name = nameof(GetContractById))]
    [ProducesResponseType(typeof(ContractDto), StatusCodes.Status200OK)]
    [EndpointSummary("Retrieves contract details by ID under a specific case.")]
    [EndpointDescription("Fetches a contract by its ID for the given case.")]
    [EndpointName(nameof(GetContractById))]
    public async Task<IActionResult> GetContractById(Guid caseId, Guid contractId, CancellationToken ct)
    {
        var result = await _query.QueryAsync(new GetContractByIdQuery(caseId,contractId), ct);
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

        var filesCommands = new List<CreateContractFileWithContractCommand>();
        foreach (var fileRequest in request.ContractFiles)
        {
            using var ms = new MemoryStream();
            await fileRequest.CopyToAsync(ms, ct);
            ms.Position = 0;

            filesCommands.Add(new CreateContractFileWithContractCommand(
                FileName: fileRequest.FileName,
                File: new MemoryStream(ms.ToArray())
            ));
        }

        var commandRequest = new CreateContractCommand(
            CaseId: caseId,
            ContractType: (Domain.Cases.Enums.ContractType)request.ContractType,
            IssuedOn: request.IssuedOn,
            ExpiresOn: request.ExpiresOn,
            TotalAmount: request.BaseAmount,
            InitialPayment: request.InitialPayment,
            IsAssigned: request.IsAssigned,
            ContractFiles: filesCommands
        );

        var result = await _command.SendAsync(commandRequest, ct);

        return result.Match(
            id => CreatedAtRoute(
                routeName: nameof(GetContractById),
                routeValues: new { caseId = caseId, contractId = id },
                value:new { contractId = id }),
            Problem
        );
    }
}
