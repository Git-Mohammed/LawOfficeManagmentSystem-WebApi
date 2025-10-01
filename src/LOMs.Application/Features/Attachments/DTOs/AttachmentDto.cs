namespace LOMs.Application.Features.Attachments.DTOs
{
    public class AttachmentDto
    {
        public Guid AttachmentId { get; set ; }
        public string AttachmentName { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedByEmployeeName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
    }
}
