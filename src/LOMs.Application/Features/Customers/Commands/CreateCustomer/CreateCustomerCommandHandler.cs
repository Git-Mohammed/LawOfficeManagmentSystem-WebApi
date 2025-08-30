using LiteBus.Commands.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Customers.Dtos;
using LOMs.Domain.Common.Results;
using LOMs.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler(
    ILogger<CreateCustomerCommandHandler> logger,
    IAppDbContext context,
    IMapper mapper)
    : ICommandHandler<CreateCustomerCommand, Result<CustomerDto>>
{
    private readonly ILogger<CreateCustomerCommandHandler> _logger = logger;
    private readonly IAppDbContext _context = context;

    public async Task<Result<CustomerDto>> HandleAsync(CreateCustomerCommand command, CancellationToken ct)
    {
        var email = command.Email.Trim().ToLower();

        var exists = await _context.Customers.AnyAsync(
            c => c.Email!.ToLower() == email,
            ct);

        if (exists)
        {
            _logger.LogWarning("Customer creation aborted. Email already exists.");

            return CustomerErrors.CustomerExists;
        }


        var createCustomerResult = Customer.Create(
            Guid.NewGuid(),
            command.Name.Trim(),
            command.PhoneNumber.Trim(),
            command.Email.Trim());

        if (createCustomerResult.IsError)
        {
            return createCustomerResult.Errors;
        }

        _context.Customers.Add(createCustomerResult.Value);

        await _context.SaveChangesAsync(ct);

        var customer = createCustomerResult.Value;

        _logger.LogInformation("Customer created successfully. Id: {CustomerId}", createCustomerResult.Value.Id);

        return mapper.Map<Customer,CustomerDto>(customer);
        //return customer.ToDto();
    }
}