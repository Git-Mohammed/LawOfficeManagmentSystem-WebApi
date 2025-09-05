using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.People.Clients.Commands.CreateClient;
using LOMs.Application.Features.People.Clients.Dtos;
using LOMs.Application.Features.People.Clients.Queries.GetClientByNationalIdQuery;
using LOMs.Contract.Requests.Clients;
using Microsoft.AspNetCore.Mvc;

namespace LOMs.Api.Controllers;

[Route("api/clients")]
public class ClientsController(ICommandMediator command, IQueryMediator query) : ApiController
{
    private readonly IQueryMediator _query = query;
    private readonly ICommandMediator _command = command;

    [HttpGet("{clientId:guid}", Name = nameof(GetClientById))]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves client details by ID.")]
    [EndpointDescription("Fetches a client and its associated person data using the client identifier.")]
    [EndpointName(nameof(GetClientById))]
    public async Task<IActionResult> GetClientById(Guid clientId, CancellationToken ct)
    {
        var queryRequest = new GetClientByIdQuery(clientId);
        var result = await _query.QueryAsync(queryRequest, ct);

        return result.Match(Ok, Problem);
    }

    [HttpGet("national-id/{nationalId}", Name = nameof(GetClientByNationalId))]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves client details by national ID.")]
    [EndpointDescription("Fetches a client using their national ID, including associated person data.")]
    [EndpointName(nameof(GetClientByNationalId))]
    public async Task<IActionResult> GetClientByNationalId(string nationalId, CancellationToken ct)
    {
        var queryRequest = new GetClientByNationalIdQuery(nationalId);
        var result = await _query.QueryAsync(queryRequest, ct);

        return result.Match(Ok, Problem);
    }

    [HttpPost(Name = nameof(CreateClient))]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Creates a new client.")]
    [EndpointDescription("Registers a new client and their associated person details.")]
    [EndpointName(nameof(CreateClient))]
    public async Task<IActionResult> CreateClient([FromBody] CreateClientRequest request, CancellationToken ct)
    {
        var personCommand = new CreatePersonCommand(
            FullName: request.Person.FullName,
            NationalId: request.Person.NationalId,
            CountryCode: request.Person.CountryCode,
            BirthDate: request.Person.BirthDate,
            PhoneNumber: request.Person.PhoneNumber,
            Address: request.Person.Address);

        var commandRequest = new CreateClientCommand(personCommand);
        var result = await _command.SendAsync(commandRequest, ct);

        return result.Match(
            response => CreatedAtRoute(
                routeName: nameof(GetClientById),
                routeValues: new { clientId = response.ClientId },
                value: response),
            Problem);
    }
}
