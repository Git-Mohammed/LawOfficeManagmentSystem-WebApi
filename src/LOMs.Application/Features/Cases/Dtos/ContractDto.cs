using LOMs.Domain.Cases.Enums;

namespace LOMs.Application.Features.Cases.Dtos
{
    public class ContractDto
    {
        public Guid ContractId { get; set; }
        public string ContractNumber { get; set; } = string.Empty;

        public ContractType ContractType { get; set; }
        public DateOnly? IssueDate { get; set; }
        public DateOnly? ExpiryDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal InitialPayment { get; set; }
        public string ContractFilePath { get; set; } = string.Empty;

        public bool IsAssigned { get; set; }
    }
}
