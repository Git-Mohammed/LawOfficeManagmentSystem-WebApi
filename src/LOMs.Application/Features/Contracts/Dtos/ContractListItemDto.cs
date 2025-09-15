using LOMs.Domain.Cases.Enums;

namespace LOMs.Application.Features.Contracts.Dtos;

public class ContractListItemDto
{
    public Guid ContractId { get; set; }
    public string ContractNumber { get; set; } = string.Empty;
    public ContractType ContractType { get; set; }
}
