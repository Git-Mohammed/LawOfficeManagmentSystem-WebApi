using System;
using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.Contracts
{
    public class CreateContractRequest
    {
        [Required(ErrorMessage = "Case ID is required.")]
        public Guid CaseId { get; set; }

        [Required(ErrorMessage = "Court type is required.")]
        public int CourtType { get; set; }

        [Required(ErrorMessage = "Contract type is required.")]
        public int Type { get; set; }

        [Required(ErrorMessage = "Issue date is required.")]
        public DateOnly IssuedOn { get; set; }

        [Required(ErrorMessage = "Expiry date is required.")]
        public DateOnly ExpiresOn { get; set; }

        [Required(ErrorMessage = "Total amount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Total amount must be greater than or equal to 0.")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Initial payment is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Initial payment must be greater than or equal to 0.")]
        public decimal InitialPayment { get; set; }

        [Required(ErrorMessage = "File path is required.")]
        public string FilePath { get; set; } = string.Empty;

        public bool IsAssigned { get; set; }
    }
}
