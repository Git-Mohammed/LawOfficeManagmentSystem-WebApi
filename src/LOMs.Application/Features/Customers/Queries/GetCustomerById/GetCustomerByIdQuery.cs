using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Customers.Dtos;
using LOMs.Domain.Common.Results;


namespace LOMs.Application.Features.Customers.Queries.GetCustomerById;

public sealed record GetCustomerByIdQuery(Guid CustomerId) : IQuery<Result<CustomerDto>>;