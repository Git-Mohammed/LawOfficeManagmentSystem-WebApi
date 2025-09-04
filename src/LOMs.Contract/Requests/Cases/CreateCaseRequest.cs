using LOMs.Contract.Commons.Enums;
using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.Cases
{
    public class CreateCaseRequest
    {
        public string CaseNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "نوع المحكمة مطلوب.")]
        public CourtType CourtType { get; set; }

        public string CaseSubject { get; set; } = string.Empty;

        [Required(ErrorMessage = "دور الطرف مطلوب.")]
        public PartyRole PartyRole { get; set; }

        public string ClientRequestDetails { get; set; } = string.Empty;

        public DateOnly? EstimatedReviewDate { get; set; }

        public bool IsDraft { get; set; }

        public bool HasContracts { get; set; }

        public List<CreateContractWithCaseRequest>? Contracts { get; set; }


        public bool HasPOAs { get; set; }

        public List<CreatePOAWithCaseRequest>? POAs { get; set; }

        public string? LawyerOpinion { get; set; }

        [Required(ErrorMessage = " الموظف المسؤول مطلوب.")]
        public Guid AssignedOfficer { get; set; } 

        [MinLength(1, ErrorMessage = "يجب إضافة عميل واحد على الأقل.")]
        public List<CaseClientRequest> Clients { get; set; } = new();
    }
}
