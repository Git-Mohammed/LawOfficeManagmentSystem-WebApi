using LOMs.Domain.Cases;
using LOMs.Domain.Common;
using LOMs.Domain.Common.Results;

namespace LOMs.Domain.POAs
{
    public sealed class POA : AuditableEntity
    {
        public Guid CaseId { get; private set; }
        public Case Case { get; private set; } = null!;
        public string POANumber { get; private set; } = string.Empty;
        public DateOnly IssueDate { get; private set; }
        public string IssuingAuthority { get; private set; } = string.Empty;
        public string AttachmentPath { get; private set; } = string.Empty;

        private POA() { }

        private POA(
            Guid id,
            Guid caseId,
            string number,
            DateOnly issueDate,
            string issuingAuthority,
            string attachmentPath
        ) : base(id)
        {
            CaseId = caseId;
            POANumber = number;
            IssueDate = issueDate;
            IssuingAuthority = issuingAuthority;
            AttachmentPath = attachmentPath;
        }

        /// <summary>
        /// Factory method to create a new POA with validation.
        /// </summary>
        public static Result<POA> Create(
            Guid id,
            Guid caseId,
            string number,
            DateOnly issueDate,
            string issuingAuthority,
            string attachmentPath
        )
        {
            if (id == Guid.Empty)
                return Error.Conflict();

            if (caseId == Guid.Empty)
                return POAErrors.Missing_CaseReference;

            if (string.IsNullOrWhiteSpace(number))
                return POAErrors.Missing_Number;

            if (string.IsNullOrWhiteSpace(issuingAuthority))
                return POAErrors.Missing_IssuingAuthority;

            if (issueDate > DateOnly.FromDateTime(DateTime.UtcNow))
                return POAErrors.Invalid_IssueDate();

            if (string.IsNullOrWhiteSpace(attachmentPath))
                return POAErrors.Missing_Attachment;

            return new POA(
                id,
                caseId,
                number.Trim(),
                issueDate,
                issuingAuthority.Trim(),
                attachmentPath.Trim()
            );
        }

        public override string ToString()
        {
            return $"POA #{POANumber} - Issued by {IssuingAuthority} on {IssueDate}";
        }
    }
}
