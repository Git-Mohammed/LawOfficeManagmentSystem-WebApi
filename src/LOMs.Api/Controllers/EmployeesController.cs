using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.People.Clients.Commands.CreateClient;
using LOMs.Application.Features.People.Employees.Commands;
using LOMs.Application.Features.People.Employees.Queries;
using LOMs.Contract.Requests.Employees;
using Microsoft.AspNetCore.Mvc;

namespace LOMs.Api.Controllers;
[Route("api/employees")]
public class EmployeesController(ICommandMediator command, IQueryMediator query):ApiController
{
    [HttpGet("id:guid", Name = "GetEmployeeById")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await query.QueryAsync(new GetEmployeeByIdQuery(id), ct);
        return result.Match(
            response => Ok(response),
            Problem
        );
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateEmployee(CreateEmployeeRequest request, CancellationToken ct)
    {
        var result = await command.SendAsync(new CreateEmployeeCommand(
            Role:  request.Role,
            Email: request.Email,
            Person: new CreatePersonCommand(
                request.Person.FullName,
                request.Person.NationalId,
                request.Person.BirthDate,
                request.Person.PhoneNumber,
                request.Person.Address
                )
            ), ct);

        return result.Match(
            response => CreatedAtRoute(
                routeName: "GetEmployeeById",
                routeValues: new {employeeId = response.Id},
                value:  response
                ),
            Problem
        );
    }
}