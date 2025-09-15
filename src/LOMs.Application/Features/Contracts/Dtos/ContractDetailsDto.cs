namespace LOMs.Application.Features.Contracts.Dtos;

public sealed class ContractDetailsDto
{
    public Guid ContractId { get; init; }
    public string ContractNumber { get; set; } = default!;
    public decimal TotalAmount { get; set; }
    public decimal InitialPayment { get; set; }
    public bool IsAssigned { get; set; }
}
