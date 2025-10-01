using LOMs.Application.Features.Judgments.Dtos;
using LOMs.Domain.Cases.Judgments;
using Mapster;

namespace LOMs.Infrastructure.Mapping.CasesMappers;

public class JudgmentMapperConfigs : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Judgment, JudgmentDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Number, src => src.Number)
            .Map(dest => dest.Type, src => src.Type.ToString())
            .Map(dest => dest.SubType, src => src.SubType.ToString())
            .Map(dest => dest.IssuedOn, src => src.IssuedOn);

    }
}