using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Customers.Dtos;
using LOMs.Application.Features.Customers.Mappers;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;

namespace LOMs.Application.Features.Customers.Queries.GetCustomers;

public class GetCustomersQueryHandler(IAppDbContext context
    )
    : IQueryHandler<GetCustomersQuery, Result<List<CustomerDto>>>
{
    private readonly IAppDbContext _context = context;

    public async Task<Result<List<CustomerDto>>> HandleAsync(GetCustomersQuery query, CancellationToken ct)
    {
        var customers = await _context.Customers.AsNoTracking().ToListAsync(ct);

        return customers.ToDtos();
    }
}