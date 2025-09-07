using LOMs.Contract.Commons.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.Cases
{
    public class CreateCaseRequest
    {
        public string? CaseNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "نوع المحكمة مطلوب.")]
        public Guid CourtTypeId { get; set; }

        public string? CaseSubject { get; set; } = string.Empty;

        [Required(ErrorMessage = "دور الطرف مطلوب.")]
        public PartyRole PartyRole { get; set; }

        public string? ClientRequestDetails { get; set; } = string.Empty;

        public DateOnly EstimatedReviewDate { get; set; }

        public bool IsDraft { get; set; }

        public string? LawyerOpinion { get; set; } = string.Empty;

        [Required(ErrorMessage = " الموظف المسؤول مطلوب.")]
        public Guid AssignedEmployeeId { get; set; }

           [MinLength(1, ErrorMessage = "يجب إضافة عميل واحد على الأقل.")]
        public List<CaseClientRequest> Clients { get; set; } = new();
    }
}
