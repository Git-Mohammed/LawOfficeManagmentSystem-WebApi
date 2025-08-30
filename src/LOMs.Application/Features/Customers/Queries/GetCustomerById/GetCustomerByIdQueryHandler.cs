using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Customers.Dtos;
using LOMs.Domain.Common.Results;
using LOMs.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQueryHandler(
    ILogger<GetCustomerByIdQueryHandler> logger,
    IAppDbContext context,
    IMapper mapper)
    : IQueryHandler<GetCustomerByIdQuery, Result<CustomerDto>>
{
    private readonly ILogger<GetCustomerByIdQueryHandler> _logger = logger;
    private readonly IAppDbContext _context = context;

    public async Task<Result<CustomerDto>> HandleAsync(GetCustomerByIdQuery query, CancellationToken ct)
    {
        var customer = await _context.Customers.AsNoTracking()
                                     .FirstOrDefaultAsync(c => c.Id == query.CustomerId, ct);

        if (customer is null)
        {
            _logger.LogWarning("Customer with id {CustomerId} was not found", query.CustomerId);

            return Error.NotFound(
                code: "Customer_NotFound",
                description: $"Customer with id '{query.CustomerId}' was not found");
        }

        return mapper.Map<Customer,CustomerDto>(customer);
        //return customer.ToDto();
    }
}