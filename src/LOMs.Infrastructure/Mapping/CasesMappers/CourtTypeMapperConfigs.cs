using LOMs.Application.Features.Cases.Dtos;
using LOMs.Domain.Cases.CourtTypes;
using Mapster;

namespace LOMs.Infrastructure.Mapping.CasesMappers;

public class CourtTypeMapperConfigs : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CourtType, CourtTypeDto>()
            .Map(dest => dest.CourtTypeId, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Code, src => src.Code)
            .Map(dest => dest.Description, src => src.Description);

    }
}
