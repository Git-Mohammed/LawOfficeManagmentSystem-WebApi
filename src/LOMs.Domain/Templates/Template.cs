using LOMs.Domain.Common.Files;
using LOMs.Domain.Common.Results;

namespace LOMs.Domain.Templates
{
    public sealed  class Template :DbFile
    {
       
        public string? TemplateName { get; private set; }

        private Template() { }

        private Template(
            Guid id,
            string tamplatename,
            string filename,
            string filepath,
            FileType fileType,
            long size,
            string extenstion,
            string contentType):base(id,filename,filepath,fileType,size,extenstion,contentType) 
        {
            TemplateName = tamplatename;
        }

        public static Result<Template> Create(
              Guid id,
             string templatename,
            string filename,
            string filepath,
            FileType fileType,
            long size,
            string extenstion,
            string contentType)
        {
            if (string.IsNullOrEmpty(templatename))
                return TemplateErrors.TemplateNameRequired;

            var validation = Validator(filename, filepath, fileType, size, extenstion, contentType);

            if (validation.IsError)
                return validation.Errors;

            return new Template(
            id, templatename, filename, filepath, fileType, size, extenstion, contentType);
        }



    }
}
