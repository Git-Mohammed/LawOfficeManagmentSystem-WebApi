using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.Cases
{
    public class CreateCaseRequest
    {
        public string CaseNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "نوع المحكمة مطلوب.")]
        public int CourtType { get; set; }

        public string CaseSubject { get; set; } = string.Empty;

        [Required(ErrorMessage = "دور الطرف مطلوب.")]
        public int PartyRole { get; set; }

        public string ClientRequestDetails { get; set; } = string.Empty;

        public DateOnly? EstimatedReviewDate { get; set; }

        public bool IsDraft { get; set; }

        public bool HasContracts { get; set; }

       public List<CreateContractWithCaseRequest>? Contracts { get; set; }

        public string? LawyerOpinion { get; set; }

        [Required(ErrorMessage = " الموظف المسؤول مطلوب.")]
        public string AssignedOfficer { get; set; } = string.Empty;

        [MinLength(1, ErrorMessage = "يجب إضافة عميل واحد على الأقل.")]
        public List<CaseClientRequest> Clients { get; set; } = new();
    }
}
