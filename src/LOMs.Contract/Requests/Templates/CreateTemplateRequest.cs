using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Contract.Requests.Templates
{
    public class CreateTemplateRequest
    {
        [Required(ErrorMessage = "Template Name is required.")]
        [StringLength(50, ErrorMessage = "Template Name cannot exceed 50 characters.")]
        public string TemplateName { get; set; } = null!;

        [Required(ErrorMessage = "File is required.")]
        public IFormFile File { get; set; } = null!;
    }
}
