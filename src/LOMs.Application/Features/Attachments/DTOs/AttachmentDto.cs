using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Application.Features.Attachments.DTOs
{
    public class AttachmentDto
    {
        public string AttachmentName { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedByEmployeeName { get; set; }
    }
}
