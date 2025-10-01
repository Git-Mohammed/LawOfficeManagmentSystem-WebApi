using LOMs.Application.Features.Templates.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Application.Features.Tamplates.DTOs
{
    public class TemplateDto
    {
        public Guid TemplateId { get; set; }
        public string TemplateName { get; set; }
        public FileDto File { get; set; }
    }
}
