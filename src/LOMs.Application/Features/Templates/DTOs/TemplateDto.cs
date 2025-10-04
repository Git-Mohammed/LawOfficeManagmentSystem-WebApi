using LOMs.Application.Features.Templates.DTOs;

namespace LOMs.Application.Features.Tamplates.DTOs
{
    public class TemplateDto
    {
        public Guid TemplateId { get; set; }
        public string? TemplateName { get; set; }
        public FileDto? File { get; set; }
    }
}
