using LOMs.Application.Features.Attachments.DTOs;
using LOMs.Domain.Cases.Attachments;
using Mapster;
using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Infrastructure.Mapping.CasesMappers
{
    public class AttachmentMapperConfigs : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Attachment, AttachmentDto>()
                .Map(dest => dest.AttachmentName, src => src.AttachmentName)
                .Map(dest => dest.CreatedDate, src => src.CreatedAtUtc)
                .Map(dest => dest.CreatedByEmployeeName, src => src.CreatedBy);
        }
    }
}
