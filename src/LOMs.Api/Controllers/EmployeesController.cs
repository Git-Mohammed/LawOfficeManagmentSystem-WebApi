using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.People.Clients.Commands.CreateClient;
using LOMs.Application.Features.People.Commands;
using LOMs.Application.Features.People.Employees.Commands;
using LOMs.Application.Features.People.Employees.Dtos;
using LOMs.Application.Features.People.Employees.Queries;
using LOMs.Application.Features.People.Employees.Queries.GetAvailableEmployeesByDate;
using LOMs.Contract.Requests.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LOMs.Api.Controllers;

[Route("api/employees")]
[Authorize]
public class EmployeesController(ICommandMediator command, IQueryMediator query) : ApiController
{
    private readonly ICommandMediator _command = command;
    private readonly IQueryMediator _query = query;

    [HttpGet("names", Name = nameof(GetEmployeeNames))]
    [ProducesResponseType(typeof(IReadOnlyList<EmployeeNameDto>), StatusCodes.Status200OK)]
    [EndpointSummary("Retrieves all employee names.")]
    [EndpointDescription("Returns a list of employee IDs and full names.")]
    [EndpointName(nameof(GetEmployeeNames))]
    public async Task<IActionResult> GetEmployeeNames(CancellationToken ct)
    {
        var result = await _query.QueryAsync(new GetEmployeeNamesQuery(), ct);
        return result.Match(Ok, Problem);
    }

    [HttpGet("available", Name = nameof(GetAvailableEmployeesByDate))]
    [ProducesResponseType(typeof(IReadOnlyList<EmployeeNameDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves available employees for a specific date.")]
    [EndpointDescription("Returns employees who are not assigned to any court session at the given date.")]
    [EndpointName(nameof(GetAvailableEmployeesByDate))]
    public async Task<IActionResult> GetAvailableEmployeesByDate(
        [FromQuery] DateTimeOffset date,
        CancellationToken ct)
    {
        var result = await _query.QueryAsync(new GetAvailableEmployeesByDateQuery(date), ct);
        return result.Match(Ok, Problem);
    }
    
    
    [HttpGet("{id:guid}", Name = nameof(GetEmployeeById))]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Retrieves employee details by ID.")]
    [EndpointDescription("Returns full employee information for the given ID.")]
    [EndpointName(nameof(GetEmployeeById))]
    public async Task<IActionResult> GetEmployeeById(Guid id, CancellationToken ct)
    {
        var result = await _query.QueryAsync(new GetEmployeeByIdQuery(id), ct);
        return result.Match(Ok, Problem);
    }

    [HttpPost(Name = nameof(CreateEmployee))]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Creates a new employee.")]
    [EndpointDescription("Registers a new employee with personal and role details.")]
    [EndpointName(nameof(CreateEmployee))]
    public async Task<IActionResult> CreateEmployee(
        [FromBody] CreateEmployeeRequest request,
        CancellationToken ct)
    {

        var person = new CreatePersonCommand(
            FullName: request.Person.FullName,
            NationalId: request.Person.NationalId,
            CountryCode: request.Person.CountryCode,
            BirthDate: request.Person.BirthDate,
            Address: request.Person.Address,
            Phone: new CreatePhoneCommand(request.Person.Phone.Number, request.Person.Phone.Code));

        var command = new CreateEmployeeCommand(
            Role:(LOMs.Domain.Identity.Role)request.Role,
            Email: request.Email,
            Person: person
        );

        var result = await _command.SendAsync(command, ct);

        return result.Match(
            response => CreatedAtRoute(
                routeName: nameof(GetEmployeeById),
                routeValues: new { id = response.Id },
                value: response
            ),
            Problem
        );
    }
    
    [HttpGet("stats", Name = nameof(GetEmployeeStats))]
    [ProducesResponseType(typeof(EmployeeStatsDto), StatusCodes.Status200OK)]
    [EndpointSummary("Retrieves employee statistics.")]
    [EndpointDescription("Returns the total number of employees, and counts of active and inactive employees.")]
    [EndpointName(nameof(GetEmployeeStats))]
    public async Task<IActionResult> GetEmployeeStats(CancellationToken ct)
    {
        var result = await _query.QueryAsync(new GetEmployeeStatsQuery(), ct);
        return result.Match(response => Ok(response), Problem);
    }


    [HttpGet(Name = nameof(GetAllEmployees))]
    [ProducesResponseType(typeof(IReadOnlyList<EmployeeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves all employees.")]
    [EndpointDescription("Returns a complete list of all employees in the system.")]
    [EndpointName(nameof(GetAllEmployees))]
    public async Task<IActionResult> GetAllEmployees(CancellationToken ct)
    {
        var result = await _query.QueryAsync(new GetAllEmployeesQuery(), ct);
        return result.Match(Ok, Problem);
    }
    
    
    
    [HttpPut(Name = nameof(UpdateEmployee))]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Updates an existing employee.")]
    [EndpointDescription("Updates employee information based on the provided request.")]
    [EndpointName(nameof(UpdateEmployee))]
    public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeRequest request, CancellationToken ct)
    {
        var command = new UpdateEmployeeCommand(
            request.Id,
            request.FullName,
            request.NationalId,
            request.BirthDate,
            request.Phone.Number,
            request.Phone.Code,
            request.Address,
            request.CountryCode,
            request.Role,
            request.Email,
            request.IsActive
        );

        var result = await _command.SendAsync(command, ct);
        return result.Match(_=>NoContent(), Problem);
    }
}
