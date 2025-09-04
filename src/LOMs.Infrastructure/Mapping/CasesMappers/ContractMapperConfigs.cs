using LOMs.Application.Features.Cases.Dtos;
using LOMs.Domain.Cases.Contracts;
using Mapster;

namespace LOMs.Infrastructure.Mapping.CasesMappers
{
    internal class ContractMapperConfigs : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Domain Contract -> ContractDto
            config.NewConfig<Contract, ContractDto>()
                .Map(dest => dest.ContractId, src => src.Id)
                .Map(dest => dest.ContractNumber, src => src.DisplayContractNumber)
                .Map(dest => dest.IssueDate, src => src.IssuedOn)
                .Map(dest => dest.ExpiryDate, src => src.ExpiresOn)
                .Map(dest => dest.TotalAmount, src => src.TotalAmount)
                .Map(dest => dest.ContractType, src => src.Type)
                .Map(dest => dest.ContractFilePath, src => src.FilePath);

        }
    }
}
