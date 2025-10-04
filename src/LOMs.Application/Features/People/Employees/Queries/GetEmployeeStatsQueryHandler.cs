using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.People.Employees.Dtos;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.People.Employees.Queries;

public class GetEmployeeStatsQueryHandler(IAppDbContext context, ILogger<GetEmployeeStatsQueryHandler> logger) : IQueryHandler<GetEmployeeStatsQuery, Result<EmployeeStatsDto>>
{
    private readonly IAppDbContext _context = context;
    private readonly ILogger<GetEmployeeStatsQueryHandler> _logger = logger;

    public async Task<Result<EmployeeStatsDto>> HandleAsync(GetEmployeeStatsQuery query, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            var stats = await _context.Employees.AsNoTracking()
                .GroupBy(e => 1) 
                .Select(g => new EmployeeStatsDto
                {
                    TotalEmployees = g.Count(x => x != null),
                    ActiveEmployees = g.Count(e => e.IsActive),
                    InactiveEmployees = g.Count(e => !e.IsActive)
                })
                .FirstOrDefaultAsync(cancellationToken);

            return stats ?? new EmployeeStatsDto();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employee stats");
            return Error.Failure("EmployeeStatsQuery.Failed", "Could not retrieve employee statistics.");
        }
    }
}