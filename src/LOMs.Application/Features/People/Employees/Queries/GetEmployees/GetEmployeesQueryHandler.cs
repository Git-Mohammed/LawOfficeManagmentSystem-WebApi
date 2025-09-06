using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.People.Employees.DTOs;
using LOMs.Domain.Common.Results;
using LOMs.Domain.People.Employees;
using Microsoft.EntityFrameworkCore;

namespace LOMs.Application.Features.People.Employees.Queries.GetEmployees;

public class GetEmployeesQueryHandler (IAppDbContext context, IMapper mapper):IQueryHandler<GetEmployeesQuery,Result<List<EmployeeShortDto>>>
{
    private readonly IAppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<EmployeeShortDto>>> HandleAsync(GetEmployeesQuery query, CancellationToken cancellationToken = new CancellationToken())
    {
        var employees = await _context.Employees.Include(p=>p.Person).AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
        if (employees == null)
            return Error.NotFound();
        return _mapper.Map<List<Employee>,List<EmployeeShortDto>>(employees);
    }
}