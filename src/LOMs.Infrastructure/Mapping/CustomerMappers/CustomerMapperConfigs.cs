using LOMs.Application.Features.Customers.Dtos;
using LOMs.Domain.Customers;
using Mapster;

namespace LOMs.Infrastructure.Mapping.CustomerMappers;

public class CustomerMapperConfigs : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Customer, CustomerDto>().Map(dest=> dest.CustomerId,  src => src.Id);
        config.NewConfig<IEnumerable<Customer>, IEnumerable<CustomerDto>>();
    }
}