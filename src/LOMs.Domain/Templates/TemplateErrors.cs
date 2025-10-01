using LOMs.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Domain.Templates
{
    public class TemplateErrors
    {
        public static readonly Error TemplateNameRequired =
           Error.Validation("Template.TemplateNameRequired", "TemplateName is Required.");
        public static readonly Error NotFound =
        Error.NotFound("Template.NotFound", "Template not found.");
        public static readonly Error MissingFilePath =
       Error.Conflict("Template.MissingFilePath", "File path is required to download the Template file.");
        public static readonly Error FileNotFound =
            Error.NotFound("Tamplate.FileNotFound", "File does not Exist ");
    }
}
