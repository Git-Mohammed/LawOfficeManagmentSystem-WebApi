using LOMs.Application.Features.Cases.Dtos;
using LOMs.Application.Features.Customers.Dtos;
using LOMs.Application.Features.People.Clients.Dtos;
using LOMs.Domain.Cases;
using LOMs.Domain.Customers;
using LOMs.Domain.People.Clients;
using Mapster;

namespace LOMs.Infrastructure.Mapping.CasesMappers;

public class CaseMapperConfigs : IRegister
{
    public void Register(TypeAdapterConfig config)
    {


        // Example: Map Case -> CaseDetailsDto (for GetCaseById)
        config.NewConfig<Case, CaseDetailsDto>()
     .Map(dest => dest.Id, src => src.Id)
     .Map(dest => dest.CaseNumber, src => src.CaseNumber)
     .Map(dest => dest.CourtType, src => src.CourtType)
     .Map(dest => dest.CaseSubject, src => src.CaseSubject)
     .Map(dest => dest.PartyRole, src => src.PartyRole)
     .Map(dest => dest.ClientRequestDetails, src => src.ClientRequests)
     .Map(dest => dest.EstimatedReviewDate, src => src.EstimatedReviewDate)
     .Map(dest => dest.CaseStatus, src => src.Status.ToString())
     .Map(dest => dest.Contracts, src => src.Contracts)
     .Map(dest => dest.LawyerOpinion, src => src.LawyerOpinion)
     .Map(dest => dest.AssignedOfficer, src => src.AssignedOfficer)
     .Map(dest => dest.HasContracts, src => src.Contracts.Any())
     .Map(dest => dest.Clients, src => src.ClientCases.Select(x => new ClientDto
     {
         ClientId = x.Client.Id,
         Person = new PersonDto
         {
             PersonId = x.Client.Person.Id,
             PhoneNumber = x.Client.Person.PhoneNumber,
             FullName = x.Client.Person.FullName,
             Address = x.Client.Person.Address,
             BirthDate = x.Client.Person.BirthDate,
             NationalId = x.Client.Person.NationalId
         }
     }).ToList());

    }
}
