using LOMs.Application.Features.Customers.Dtos;
using LOMs.Application.Features.Customers.Mappers;
using LOMs.Domain.Customers;
using LOMs.Domain.Customers.Vehicles;


namespace LOMs.Application.Features.Customers.Mappers;

public static class CustomerMapper
{
    public static CustomerDto ToDto(this Customer entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new CustomerDto
        {
            CustomerId = entity.Id,
            Name = entity.Name!,
            Email = entity.Email!,
            PhoneNumber = entity.PhoneNumber!
        };
    }

    public static List<CustomerDto> ToDtos(this IEnumerable<Customer> entities)
    {
        return [.. entities.Select(e => e.ToDto())];
    }
}