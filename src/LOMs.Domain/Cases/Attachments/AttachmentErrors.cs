using LOMs.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Domain.Cases.Attachments
{
    public class AttachmentErrors
    {
        public static readonly Error CaseIdRequired =
      Error.Validation("Attachment.CaseIdRequired", "CaseId is required.");

        public static readonly Error AttachmentNameRequired =
            Error.Validation("Attachment.AttachmentNameRequired", "AttachmentName is Required.");
        public static readonly Error NotFound =
        Error.NotFound("Attachment.NotFound", "Attachment not found.");
    }
}
