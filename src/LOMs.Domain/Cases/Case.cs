using LOMs.Domain.Cases.ClientFiles;
using LOMs.Domain.Cases.Contracts;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Common;
using LOMs.Domain.Common.Results;
using LOMs.Domain.People.Clients;
using LOMs.Domain.People.Employees;
using LOMs.Domain.POAs;

namespace LOMs.Domain.Cases
{
    public sealed class Case : AuditableEntity
    {
        public string? CaseNumber { get; private set; }
        public string? CaseSubject { get; private set; }
        public PartyRole PartyRole { get; private set; }
        public string? ClientRequests { get; private set; }
        public DateOnly? EstimatedReviewDate { get; private set; }
        public CaseStatus Status { get; private set; }
        public string? LawyerOpinion { get; private set; }
        public Guid AssignedEmployeeId { get; private set; } 
        public CourtType CourtType { get; private set; }
        public ICollection<Contract> Contracts {  get;  set; } = new List<Contract>();
        public ICollection<ClientCase> ClientCases { get;  set; } = new List<ClientCase>();
        public ICollection<POA> POAs { get; set; } = new List<POA>();
        public Employee Employee { get; set; } 




        private Case() { }

        private Case(
            Guid id,
            string? caseNumber,
            string? caseSubject,
            PartyRole partyRole,
            string? clientRequests,
            DateOnly? estimatedReviewDate,
            CaseStatus status,
            string? lawyerOpinion,
            Guid assignedEmployee,
            CourtType courtType
        ) : base(id)
        {
            CaseNumber = caseNumber;
            CaseSubject = caseSubject;
            PartyRole = partyRole;
            ClientRequests = clientRequests;
            EstimatedReviewDate = estimatedReviewDate;
            Status = status;
            LawyerOpinion = lawyerOpinion;
            AssignedEmployeeId = assignedEmployee;
            CourtType = courtType;
        }

        /// <summary>
        /// Factory method to create a new Case with validation.
        /// </summary>
        public static Result<Case> Create(
            Guid id,
            string? caseNumber,
            CourtType courtType,
            string? caseNotes,
            PartyRole role,
            string? clientRequests,
            DateOnly? estimatedReviewDate,
            CaseStatus status,
            string? lawyerOpinion,
            Guid assignedEmployeeId
        )
        {
            // Basic validations
            if (id == Guid.Empty)
                return Error.Conflict();

            if (assignedEmployeeId == Guid.Empty)
                return CaseErrors.Missing_AssignedOfficer;

            if (!Enum.IsDefined(typeof(CaseStatus), status))
                return CaseErrors.Invalid_ReviewDate;

            if (!Enum.IsDefined(typeof(PartyRole), role))
                return CaseErrors.Invalid_ReviewDate;

            if (!Enum.IsDefined(typeof(CourtType), courtType))
                return CaseErrors.Invalid_CourtType;

            return new Case(
                id,
                caseNumber,
                caseNotes,
                role,
                clientRequests,
                estimatedReviewDate,
                status,
                lawyerOpinion,
                assignedEmployeeId,
                courtType
            ); ;
        }


        public override string ToString()
        {
            return $"Case #{CaseNumber ?? "N/A"} - {PartyRole} - {CourtType} - Status: {Status}";
        }
    }
}
