using LOMs.Domain.Cases.ClientFiles;
using LOMs.Domain.Cases.Contracts;
using LOMs.Domain.Cases.CourtTypes;
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
        public DateOnly EstimatedReviewDate { get; private set; }
        public CaseStatus Status { get; private set; }
        public string? LawyerOpinion { get; private set; }
        public Guid AssignedEmployeeId { get; private set; } 
        public Guid CourtTypeId { get; private set; }
        public ICollection<Contract> Contracts {  get;  set; } = new List<Contract>();
        public ICollection<ClientCase> ClientCases { get;  set; } = new List<ClientCase>();
        public ICollection<POA> POAs { get; set; } = new List<POA>();
        public Employee? Employee { get; set; }
       public CourtType? CourtType { get; set; }

        private Case() { }

        private Case(
            Guid id,
            Guid courtTypeId,
            string? caseNumber,
            string? caseSubject,
            PartyRole partyRole,
            string? clientRequests,
            DateOnly estimatedReviewDate,
            CaseStatus status,
            string? lawyerOpinion,
            Guid assignedEmployeeId
        ) : base(id)
        {
            CaseNumber = caseNumber;
            CaseSubject = caseSubject;
            PartyRole = partyRole;
            ClientRequests = clientRequests;
            EstimatedReviewDate = estimatedReviewDate;
            Status = status;
            LawyerOpinion = lawyerOpinion;
            AssignedEmployeeId = assignedEmployeeId;
            CourtTypeId = courtTypeId;
        }

        /// <summary>
        /// Factory method to create a new Case with validation.
        /// </summary>
        public static Result<Case> Create(
           Guid id,
            Guid courtTypeId,
            string? caseNumber,
            string? caseSubject,
            PartyRole partyRole,
            string? clientRequests,
            DateOnly estimatedReviewDate,
            CaseStatus status,
            string? lawyerOpinion,
            Guid assignedEmployeeId
        )
        {
            if (id == Guid.Empty)
                return CaseErrors.InvalidCaseId;

            if (courtTypeId == Guid.Empty)
                return CaseErrors.InvalidCourtTypeId(courtTypeId);

            if (assignedEmployeeId == Guid.Empty)
                return CaseErrors.EmptyEmployeeId;

            if(estimatedReviewDate < DateOnly.FromDateTime(DateTime.UtcNow))
                return CaseErrors.InvalidReviewDate;

            if (!Enum.IsDefined(typeof(CaseStatus), status))
                return CaseErrors.InvalidStatus;

            if (!Enum.IsDefined(typeof(PartyRole), partyRole))
                return CaseErrors.InvalidPartyRole;


            return new Case(
                 id,
             courtTypeId,
            caseNumber,
             caseSubject,
             partyRole,
             clientRequests,
            estimatedReviewDate,
             status,
             lawyerOpinion,
             assignedEmployeeId
            ); ;
        }


        public override string ToString()
        {
            return $"Case #{CaseNumber ?? "N/A"} - {PartyRole} -  Status: {Status}";
        }
    }
}
