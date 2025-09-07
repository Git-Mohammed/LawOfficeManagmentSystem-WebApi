using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.People.Employees.DTOs;
using LOMs.Domain.Common.Results;
using LOMs.Domain.People.Employees;
using Microsoft.EntityFrameworkCore;

namespace LOMs.Application.Features.People.Employees.Queries;

public class GetEmployeeNamesQueryHandler(IAppDbContext context, IMapper mapper)
    : IQueryHandler<GetEmployeeNamesQuery, Result<List<EmployeeNameDto>>>
{
    private readonly IAppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<EmployeeNameDto>>> HandleAsync(GetEmployeeNamesQuery query, CancellationToken cancellationToken = new CancellationToken())
    {
        var  employees = await _context.Employees
            .AsNoTracking()
            .Include(e=> e.Person)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<Employee>, List<EmployeeNameDto>>(employees);
    }
}