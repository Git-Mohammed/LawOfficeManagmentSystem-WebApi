using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Contract.Requests.Attachments
{
    public class CreateAttachmentRequest
    {
        [Required(ErrorMessage = "Attachment Name is required.")]
        [StringLength(50, ErrorMessage = "Attachment Name cannot exceed 50 characters.")]
        public string AttachmentName { get; set; } = null!;

        [Required(ErrorMessage = "Attachment type is required.")]
        [StringLength(30, ErrorMessage = "Attachment type cannot exceed 30 characters.")]
        public string Type { get; set; } = null!;

        [Required(ErrorMessage = "File is required.")]
        public IFormFile File { get; set; } = null!;
    }
}
