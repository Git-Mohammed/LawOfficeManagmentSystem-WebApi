using LOMs.Domain.Cases.Enums;

namespace LOMs.Application.Features.Contracts.Dtos;

public class ContractListItemDto
{
    public Guid ContractId { get; set; }
    public string ContractNumber { get; set; } = string.Empty;

    public ContractType ContractType { get; set; }
    public DateOnly? IssueDate { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal InitialPayment { get; set; }

    public bool IsAssigned { get; set; }

    public string EmployeeCreatedBy { get; set; } = string.Empty;
    public DateTimeOffset CreatedDate { get; set; }
}
