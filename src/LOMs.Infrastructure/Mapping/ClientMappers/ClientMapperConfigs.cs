using LOMs.Application.Features.Customers.Dtos;
using LOMs.Application.Features.People.Clients.Dtos;
using LOMs.Domain.Customers;
using LOMs.Domain.People.Clients;
using Mapster;

namespace LOMs.Infrastructure.Mapping.ClientMappers;

public class ClientMapperConfigs : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Client, ClientDto>()
            .Map(dest => dest.ClientId, src => src.Id)
            .Map(dest => dest.Person.PersonId, src => src.Person.Id);
    }
}