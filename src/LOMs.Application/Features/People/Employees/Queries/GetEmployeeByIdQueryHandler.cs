using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.People.Employees.DTOs;
using LOMs.Domain.Common.Results;
using LOMs.Domain.People.Employees;
using Microsoft.EntityFrameworkCore;

namespace LOMs.Application.Features.People.Employees.Queries;

public class GetEmployeeByIdQueryHandler(IAppDbContext context, IMapper mapper)
    : IQueryHandler<GetEmployeeByIdQuery, Result<EmployeeDto>>
{
    private readonly IAppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<EmployeeDto>> HandleAsync(GetEmployeeByIdQuery query, CancellationToken cancellationToken = new CancellationToken())
    {
        var  employee = await _context.Employees.Include(e=> e.Person).AsNoTracking().FirstOrDefaultAsync(e=> e.Id == query.Id, cancellationToken);
        if (employee == null)
            return Error.NotFound();
        return _mapper.Map<Employee, EmployeeDto>(employee);
    }
}