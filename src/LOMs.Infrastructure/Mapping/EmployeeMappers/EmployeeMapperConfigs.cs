using LOMs.Application.Features.People.Clients.Dtos;
using LOMs.Application.Features.People.Employees.DTOs;
using LOMs.Domain.People;
using LOMs.Domain.People.Employees;
using Mapster;

namespace LOMs.Infrastructure.Mapping.EmployeeMappers;

public class EmployeeMapperConfigs: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Person -> PersonDto
        config.NewConfig<Person, PersonDto>()
            .Map(dest => dest.PersonId, src => src.Id);

        // Employee -> EmployeeDto
        config.NewConfig<Employee, EmployeeDto>()
            .Map(dest => dest.Role, src => src.Role.ToString())
            .Map(dest => dest.Person, src => src.Person);
    }
}