using LOMs.Application.Features.People.Clients.Dtos;
using LOMs.Domain.Cases.Enums;

public sealed class CaseDetailsDto
{
    public Guid CaseId { get; init; }
    public string? CaseNumber { get; init; }
    public string? CourtTypeName { get; init; }
    public string? AssignedEmployeeName { get; init; }
    public CaseStatus Status { get; init; }

    public List<CaseClientDto> Clients { get; init; } = [];
}
