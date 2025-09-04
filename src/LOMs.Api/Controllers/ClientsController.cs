using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.People.Clients.Commands.CreateClient;
using LOMs.Contract.Requests.Clients;
using Microsoft.AspNetCore.Mvc;

namespace LOMs.Api.Controllers
{
    [Route("api/clients")]
    public class ClientsController(ICommandMediator command, IQueryMediator query) : ApiController
    {
        //[HttpGet]
        //public async Task<IActionResult> Get(CancellationToken ct)
        //{
        //    var result = await query.QueryAsync(new GetClientsQuery(), ct);

        //    return result.Match(
        //        response => Ok(response),
        //        Problem);
        //}

        //[HttpGet("{clientId:guid}", Name = "GetClientById")]
        //public async Task<IActionResult> GetById(Guid clientId, CancellationToken ct)
        //{
        //    var result = await query.QueryAsync(new GetClientByIdQuery(clientId), ct);

        //    return result.Match(
        //        response => Ok(response),
        //        Problem);
        //}

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientRequest request, CancellationToken ct)
        {
            var personCommand = new CreatePersonCommand(request.Person.FullName, request.Person.NationalId, request.Person.CountryCode ,request.Person.BirthDate, request.Person.PhoneNumber, request.Person.Address);

            var result = await command.SendAsync(
                new CreateClientCommand(personCommand),
                ct);

            return result.Match(
                response => CreatedAtRoute(
                    routeName: "GetClientById",
                    routeValues: new { clientId = response.ClientId },
                    value: response),
                Problem);
        }
    }
}
