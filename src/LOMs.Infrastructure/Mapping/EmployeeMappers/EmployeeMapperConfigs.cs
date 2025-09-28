using LOMs.Application.Features.People.Clients.Dtos;
using LOMs.Application.Features.People.Employees.Dtos;
using LOMs.Domain.People;
using LOMs.Domain.People.Employees;
using LOMs.Domain.People.Phones;
using Mapster;

namespace LOMs.Infrastructure.Mapping.EmployeeMappers;

public class EmployeeMapperConfigs: IRegister
{
    public void Register(TypeAdapterConfig config)
    {

        config.NewConfig<Phone, PhoneDto>()
            .Map(dest => dest.PhoneId, src => src.Id)
            .Map(dest => dest.Number, src => src.Number)
            .Map(dest => dest.Code, src => src.Code);

        // Person -> PersonDto
        config.NewConfig<Person, PersonDto>()
            .Map(dest => dest.PersonId, src => src.Id)
            .Map(dest => dest.CountryCode, src => src.CountryCode.ToString())
                     .Map(dest => dest.Phone, src => src.Phones != null && src.Phones.Any()
                ? new PhoneDto
                {
                    PhoneId = src.Phones.First().Id,
                    Number = src.Phones.First().Number,
                    Code = src.Phones.First().Code
                }
                : new PhoneDto { Number = string.Empty, Code = string.Empty });


        // Employee -> EmployeeDto
        config.NewConfig<Employee, EmployeeDto>()
            .Map(dest => dest.Role, src => src.Role.ToString())
            .Map(dest => dest.Person, src => src.Person)
                     .Map(dest => dest.Person.Phone, src => src.Person.Phones != null && src.Person.Phones.Any()
                ? new PhoneDto
                {
                    PhoneId = src.Person.Phones.First().Id,
                    Number = src.Person.Phones.First().Number,
                    Code = src.Person.Phones.First().Code
                }
                : new PhoneDto { Number = string.Empty, Code = string.Empty })

            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.IsActive, src => src.IsActive);

        // Employee -> EmployeeDto
        config.NewConfig<Employee, EmployeeNameDto>()
            .Map(dest => dest.EmployeeId, src => src.Id)
            .Map(dest => dest.FullName, src => src.Person.FullName);

    }
}