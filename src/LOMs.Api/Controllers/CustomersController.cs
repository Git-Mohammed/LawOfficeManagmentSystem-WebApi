using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Customers.Commands.CreateCustomer;
using LOMs.Application.Features.Customers.Queries.GetCustomerById;
using LOMs.Application.Features.Customers.Queries.GetCustomers;
using LOMs.Contract.Requests.Customers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LOMs.Api.Controllers
{
    [Route("api/customers")]
    public class CustomersController : ApiController
    {
        private readonly ICommandMediator _command;
        private readonly IQueryMediator _query;

        public CustomersController(ICommandMediator command, IQueryMediator query)
        {
           _command = command;
            _query = query;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken ct)
        {
            var result = await _query.QueryAsync(new GetCustomersQuery(), ct);

            return result.Match(
                response => Ok(response),
                Problem);
        }

        [HttpGet("{customerId:guid}", Name = "GetCustomerById")]
        public async Task<IActionResult> GetById(Guid customerId, CancellationToken ct)
        {
            var result = await _query.QueryAsync(new GetCustomerByIdQuery(customerId), ct);
            return result.Match(
                response => Ok(response),
                Problem);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request, CancellationToken ct)
        {
            var result = await _command.SendAsync(
                new CreateCustomerCommand(
                request.Name,
                request.PhoneNumber,
                request.Email),
                ct);

            return result.Match(
                response => CreatedAtRoute(
                    routeName: "GetCustomerById",
                    routeValues: new { customerId = response.CustomerId },
                    value: response),
                Problem);
        }
    }
};