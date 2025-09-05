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

        public bool HasContracts { get; set; }
        // This will hold the actual files. The order must match the ContractsData list.
        public List<IFormFile>? ContractFiles { get; set; }

        public List<CreateContractWithCaseRequest> ContractsData { get; set; } = new();


        public bool HasPOAs { get; set; }

        public List<CreatePOAWithCaseRequest> POAsData { get; set; } = new();
        public List<IFormFile>? POAFiles { get; set; }

        public string? LawyerOpinion { get; set; } = string.Empty;

        [Required(ErrorMessage = " الموظف المسؤول مطلوب.")]
        public Guid AssignedEmployeeId { get; set; }

        //   [MinLength(1, ErrorMessage = "يجب إضافة عميل واحد على الأقل.")]
        public string ClientsJson { get; set; } = null!;
    }
}
