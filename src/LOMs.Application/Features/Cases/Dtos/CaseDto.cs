using LOMs.Application.Features.ClientFiles.Dtos;
using LOMs.Application.Features.People.Clients.Dtos;
using LOMs.Domain.Cases.Enums;

namespace LOMs.Application.Features.Cases.Dtos;

public class CaseDto
{
    public Guid Id { get; set; }
    public Guid ClientFileId { get; set; }

    public string? CaseNumber { get; set; }
    public string? CaseSubject { get; set; }
    public PartyRole PartyRole { get; set; }
    public string? ClientRequestDetails { get; set; }
    public DateOnly? EstimatedReviewDate { get; set; }
    public CaseStatus CaseStatus { get; set; }
    public string? LawyerOpinion { get; set; }
    public string AssignedOfficer { get; set; } = null!;
}
