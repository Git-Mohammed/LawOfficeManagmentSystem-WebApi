using LiteBus.Commands.Abstractions;
using LOMs.Application.Features.Customers.Dtos;
using LOMs.Domain.Common.Results;


namespace LOMs.Application.Features.Customers.Commands.CreateCustomer;

public sealed record CreateCustomerCommand(
    string Name,
    string PhoneNumber,
    string Email
) : ICommand<Result<CustomerDto>>;