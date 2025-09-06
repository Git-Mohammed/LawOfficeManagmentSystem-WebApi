using LOMs.Application.Features.Cases.Dtos;
using LOMs.Application.Features.Customers.Dtos;
using LOMs.Application.Features.People.Clients.Dtos;
using LOMs.Domain.Cases;
using LOMs.Domain.Customers;
using LOMs.Domain.People;
using LOMs.Domain.People.Clients;
using LOMs.Domain.POAs;
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
     .Map(dest => dest.CaseSubject, src => src.CaseSubject)
     .Map(dest => dest.PartyRole, src => src.PartyRole.ToString())
     .Map(dest => dest.ClientRequestDetails, src => src.ClientRequests)
     .Map(dest => dest.EstimatedReviewDate, src => src.EstimatedReviewDate)
     .Map(dest => dest.CaseStatus, src => src.Status.ToString())
     .Map(dest => dest.Contracts, src => src.Contracts)
     .Map(dest => dest.CourtType, src => src.CourtType)
     .Map(dest => dest.LawyerOpinion, src => src.LawyerOpinion)
     .Map(dest => dest.AssignedEmployeeId, src => src.AssignedEmployeeId)
     .Map(dest => dest.Employee, src => src.Employee)
     .Map(dest => dest.HasContracts, src => src.Contracts.Any())
     .Map(dest => dest.HasPOAs, src => src.POAs.Any())
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
             NationalId = x.Client.Person.NationalId,
             CountryCode = x.Client.Person.CountryCode.ToString()
             
         }
     })
     .ToList());
        // Domain Contract -> ContractDto
        config.NewConfig<POA, POADto>()
            .Map(dest => dest.POAId, src => src.Id)
            .Map(dest => dest.IssueDate, src => src.IssueDate)
            .Map(dest => dest.AttachmentPath, src => src.AttachmentPath)
            .Map(dest => dest.IssuingAuthority, src => src.IssuingAuthority);

    }
}
