using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Customers.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Customers.Queries.GetCustomers;

public sealed record GetCustomersQuery : IQuery<Result<List<CustomerDto>>>;