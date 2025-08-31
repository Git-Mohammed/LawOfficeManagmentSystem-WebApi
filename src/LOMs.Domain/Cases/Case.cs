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
        public Guid ClientFileId { get; }

        public string? CaseNumber { get; private set; }
        public string? CaseNotes { get; private set; }
        public PartyRole Role { get; private set; }
        public string? ClientRequests { get; private set; }
        public DateOnly? EstimatedReviewDate { get; private set; }
        public CaseStatus Status { get; private set; }
        public string? LawyerOpinion { get; private set; }
        public string AssignedOfficer { get; private set; } = null!;
        public CourtType CourtType { get; private set; }

        public ClientFile? ClientFile { get; set; }
        public ICollection<ClientCase> CaseClients { get; private set; } = new List<ClientCase>();
        private Case() { }

        private Case(
            Guid id,
            Guid clientFileId,
            string? caseNumber,
            string caseNotes,
            PartyRole role,
            string clientRequests,
            DateOnly? estimatedReviewDate,
            CaseStatus status,
            string? lawyerOpinion,
            string assignedOfficer,
            CourtType courtType
        ) : base(id)
        {
            ClientFileId = clientFileId;
            CaseNumber = caseNumber;
            CaseNotes = caseNotes;
            Role = role;
            ClientRequests = clientRequests;
            EstimatedReviewDate = estimatedReviewDate;
            Status = status;
            LawyerOpinion = lawyerOpinion;
            AssignedOfficer = assignedOfficer;
            CourtType = courtType;
        }

        public static Result<Case> Create(
            Guid id,
            Guid clientFileId,
            string? caseNumber,
            CourtType courtType,
            string caseNotes,
            PartyRole role,
            string clientRequests,
            DateOnly? estimatedReviewDate,
            CaseStatus status,
            string? lawyerOpinion,
            string assignedOfficer
        )
        {
            if (id == Guid.Empty)
                return Error.Conflict();


            if (clientFileId == Guid.Empty)
                return CaseErrors.Invalid_ClientFileId;

            if (string.IsNullOrWhiteSpace(assignedOfficer))
                return CaseErrors.Missing_AssignedOfficer;
            if (!Enum.IsDefined(typeof(CaseStatus), status))
                return CaseErrors.Invalid_ReviewDate;
            if (!Enum.IsDefined(typeof(PartyRole), role))
                return CaseErrors.Invalid_ReviewDate;
            if (!Enum.IsDefined(typeof(CourtType), courtType))
                return CaseErrors.Invalid_CourtType;

            return new Case(
                id,
                clientFileId,
                caseNumber,
                caseNotes,
                role,
                clientRequests,
                estimatedReviewDate,
                status,
                lawyerOpinion,
                assignedOfficer,
                courtType
            );
        }
    }
}
