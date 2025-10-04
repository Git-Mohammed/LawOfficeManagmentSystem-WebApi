using LOMs.Application.Features.Templates.DTOs;
using LOMs.Domain.Cases.Enums;

namespace LOMs.Application.Features.Contracts.Dtos;

public sealed class ContractDetailsDto
{
    public Guid ContractId { get; set; }
    public Guid CaseId { get; set; }
    public string ContractNumber { get; set; } = string.Empty;

    public ContractType ContractType { get; set; }
    public DateOnly? IssueDate { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public decimal TotalAmount { get; set; }

    public  FileDto? File { get; set; } 
    public Guid EmployeeIdCreatedBy { get; set; } 
    public string EmployeeNameCreatedBy { get; set; } = string.Empty;
    public DateTimeOffset CreatedDate { get; set; }

}
