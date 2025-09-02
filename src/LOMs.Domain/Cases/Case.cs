using LOMs.Domain.Cases.ClientFiles;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Cases.Enums.CourtTypes;
using LOMs.Domain.Common;
using LOMs.Domain.Common.Results;
using LOMs.Domain.People.Clients;

namespace LOMs.Domain.Cases
{
    public sealed class Case : AuditableEntity
    {
        public string? Number { get; private set; }
        public string? Subject { get; private set; }
        public PartyRole Role { get; private set; }
        public string? ClientRequests { get; private set; }
        public DateOnly? EstimatedReviewDate { get; private set; }
        public CaseStatus Status { get; private set; }
        public string? LawyerOpinion { get; private set; }
        public string AssignedOfficer { get; private set; } = null!;
        public CourtType CourtType { get; private set; }

        public ICollection<ClientCase> CaseClients { get; private set; } = new List<ClientCase>();

        private Case() { }

        private Case(
            Guid id,
            string? caseNumber,
            string? subject,
            PartyRole role,
            string? clientRequests,
            DateOnly? estimatedReviewDate,
            CaseStatus status,
            string? lawyerOpinion,
            string assignedOfficer,
            CourtType courtType
        ) : base(id)
        {
            Number = caseNumber;
            Subject = subject;
            Role = role;
            ClientRequests = clientRequests;
            EstimatedReviewDate = estimatedReviewDate;
            Status = status;
            LawyerOpinion = lawyerOpinion;
            AssignedOfficer = assignedOfficer;
            CourtType = courtType;
        }

        /// <summary>
        /// Factory method to create a new Case with validation.
        /// </summary>
        public static Result<Case> Create(
            Guid id,
            string? number,
            CourtType courtType,
            string? caseNotes,
            PartyRole role,
            string? clientRequests,
            DateOnly? estimatedReviewDate,
            CaseStatus status,
            string? lawyerOpinion,
            string assignedOfficer
        )
        {
            // Basic validations
            if (id == Guid.Empty)
                return Error.Conflict();

            if (string.IsNullOrWhiteSpace(assignedOfficer))
                return CaseErrors.Missing_AssignedOfficer;

            if (!Enum.IsDefined(typeof(CaseStatus), status))
                return CaseErrors.Invalid_ReviewDate;

            if (!Enum.IsDefined(typeof(PartyRole), role))
                return CaseErrors.Invalid_ReviewDate;

            if (!Enum.IsDefined(typeof(CourtType), courtType))
                return CaseErrors.Invalid_CourtType;

            var @case = new Case(
                id,
                number,
                caseNotes,
                role,
                clientRequests,
                estimatedReviewDate,
                status,
                lawyerOpinion,
                assignedOfficer,
                courtType
            );

            return @case;
        }


        public override string ToString()
        {
            return $"Case #{Number ?? "N/A"} - {Role} - {CourtType} - Status: {Status}";
        }
    }
}
