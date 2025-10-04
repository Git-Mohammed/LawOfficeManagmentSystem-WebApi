using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.People.Employees.Dtos;
using LOMs.Domain.Common.Results;
using LOMs.Domain.People.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.People.Employees.Queries;

public class GetAllEmployeesQueryHandler(
    IAppDbContext context,
    IMapper mapper,
    ILogger<GetAllEmployeesQueryHandler> logger)
    : IQueryHandler<GetAllEmployeesQuery, Result<IReadOnlyList<GetAllEmployeesDto>>>
{
    private readonly IAppDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GetAllEmployeesQueryHandler> _logger = logger;

    public async Task<Result<IReadOnlyList<GetAllEmployeesDto>>> HandleAsync(GetAllEmployeesQuery query, CancellationToken cancellationToken = new CancellationToken())
    {
        var employees = await _context.Employees.Include(e => e.Person).ThenInclude(p=>p!.Phones!)
            .AsNoTracking().ToListAsync(cancellationToken);
        
        if (employees.Count == 0)
        {
            _logger.LogInformation("No employees were found in the database.");
            return Error.NotFound("Employee.NotFound","There are no employees");
        }
        
        var employeesDto = _mapper.Map<List<Employee>,List<GetAllEmployeesDto>>(employees);
        return employeesDto;
        
    }
}