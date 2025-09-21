using LOMs.Domain.Cases;
using LOMs.Domain.Cases.POAs;
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

        private readonly List<POAFile> _poaFiles = [];
        public IEnumerable<POAFile> POAFiles => _poaFiles.AsReadOnly();
        private POA() { }

        private POA(
            Guid id,
            Guid caseId,
            string number,
            DateOnly issueDate,
            string issuingAuthority,
            List<POAFile> poaFiles

        ) : base(id)
        {
            CaseId = caseId;
            POANumber = number;
            IssueDate = issueDate;
            IssuingAuthority = issuingAuthority;
            _poaFiles = poaFiles;
        }

        /// <summary>
        /// Factory method to create a new POA with validation.
        /// </summary>
        public static Result<POA> Create(
            Guid caseId,
            string number,
            DateOnly issueDate,
            string issuingAuthority,
            List<POAFile> poaFiles
        )
        {
            if (caseId == Guid.Empty)
                return POAErrors.MissingCaseReference;

            if (string.IsNullOrWhiteSpace(number))
                return POAErrors.MissingNumber;

            if (string.IsNullOrWhiteSpace(issuingAuthority))
                return POAErrors.MissingIssuingAuthority;

            if (issueDate > DateOnly.FromDateTime(DateTime.UtcNow))
                return POAErrors.InvalidIssueDate();

            if (poaFiles == null || poaFiles.Count == 0)
                return POAErrors.MissingAttachment;

            return new POA(
                Guid.NewGuid(),
                caseId,
                number.Trim(),
                issueDate,
                issuingAuthority.Trim(),
                poaFiles
            );
        }


        public void AddPOAFile(POAFile poaFile)
        {
            if (poaFile == null)
                throw new ArgumentNullException(nameof(poaFile));
            _poaFiles.Add(poaFile);
        }
        public override string ToString()
        {
            return $"POA #{POANumber} - Issued by {IssuingAuthority} on {IssueDate}";
        }
    }
}
