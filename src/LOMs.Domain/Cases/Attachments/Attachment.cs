using LOMs.Domain.Common.Files;
using LOMs.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Domain.Cases.Attachments
{
    public sealed class Attachment : DbFile
    {

        public Guid CaseId { get; private set; }
        public string? AttachmentName { get; private set; }

        public Case? Case { get;  set; }

        private Attachment()
        {

        }

        private Attachment(
            Guid id,
            Guid caseId,
            string attachmentName,
            string filename,
            string filepath,
            FileType fileType,
            long size,
            string extenstion,
            string contentType)
            : base(id, filename, filepath, fileType, size, extenstion, contentType)
        {

            CaseId = caseId;
            AttachmentName = attachmentName;
        }

        public static Result<Attachment> Create(
            Guid id,
            Guid caseId,
             string attachmentName,
            string filename,
            string filepath,
            FileType fileType,
            long size,
            string extenstion,
            string contentType)
        {
            if (caseId == Guid.Empty)
                return AttachmentErrors.CaseIdRequired;

            if (string.IsNullOrEmpty(attachmentName))
                return AttachmentErrors.AttachmentNameRequired;

            var validation = Validator(filename, filepath, fileType, size, extenstion, contentType);

            if (validation.IsError)
                return validation.Errors;

            return new Attachment(
                id,
                caseId, attachmentName, filename, filepath, fileType, size, extenstion, contentType);
        }
    }
}
