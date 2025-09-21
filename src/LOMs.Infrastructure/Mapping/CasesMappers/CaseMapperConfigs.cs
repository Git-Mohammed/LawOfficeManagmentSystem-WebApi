using LOMs.Application.Features.Cases.Dtos;
using LOMs.Application.Features.People.Clients.Dtos;
using LOMs.Application.Features.POAs.Dtos;
using LOMs.Domain.Cases;
using LOMs.Domain.POAs;
using Mapster;

namespace LOMs.Infrastructure.Mapping.CasesMappers;

public class CaseMapperConfigs : IRegister
{
    public void Register(TypeAdapterConfig config)
    {


        // Example: Map Case -> CaseDetailsDto (for GetCaseById)
        config.NewConfig<Case, CaseDto>()
        .Map(dest => dest.Id, src => src.Id)
        .Map(dest => dest.CaseNumber, src => src.CaseNumber)
        .Map(dest => dest.CaseSubject, src => src.CaseSubject)
        .Map(dest => dest.PartyRole, src => src.PartyRole)
        .Map(dest => dest.ClientRequestDetails, src => src.ClientRequests)
        .Map(dest => dest.EstimatedReviewDate, src => src.EstimatedReviewDate)
        .Map(dest => dest.CaseStatus, src => src.Status);

        // Domain Case → CaseDetailsDto
        config.NewConfig<Case, CaseDetailsDto>()
            .Map(dest => dest.CaseId, src => src.Id)
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.AssignedEmployeeName, src => src.Employee.Person.FullName ?? string.Empty)
            .Map(dest => dest.CourtTypeName, src => src.CourtType.Name ?? string.Empty)
            .Map(dest => dest.Clients, src => src.ClientCases
                .Where(cc => cc.Client.Person != null)
                .Select(cc => new CaseClientDto
                {
                    ClientId = cc.ClientId,
                    FullName = cc.Client!.Person!.FullName,
                    NationalId = cc.Client.Person.NationalId,
                    PhoneNumber = cc.Client.Person.PhoneNumber,
                    CountryCode = cc.Client.Person.CountryCode.ToString()
                }).ToList());
        // Domain Case → CaseQuickStatsDto
        config.NewConfig<Case, CaseQuickStatsDto>()
            .Map(dest => dest.CaseId, src => src.Id)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.FileNumber,
                 src => src.ClientCases.FirstOrDefault().ClientFile.FileNumber ?? string.Empty)
            .Map(dest => dest.ClientName,
                 src => src.ClientCases.FirstOrDefault().Client.Person.FullName ?? string.Empty)
            .Map(dest => dest.CaseNumber,
                 src => src.CaseNumber ?? string.Empty);

        // Domain Contract -> ContractDto
        config.NewConfig<POA, POADto>()
            .Map(dest => dest.POAId, src => src.Id)
            .Map(dest => dest.IssueDate, src => src.IssueDate)
            .Map(dest => dest.IssuingAuthority, src => src.IssuingAuthority);

    }
}
