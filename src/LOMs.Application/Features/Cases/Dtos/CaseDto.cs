using LOMs.Application.Features.ClientFiles.Dtos;
using LOMs.Application.Features.People.Clients.Dtos;
using LOMs.Domain.Cases.Enums;

namespace LOMs.Application.Features.Cases.Dtos;

public class CaseDto
{
    public Guid Id { get; set; }
    public Guid ClientFileId { get; set; }

    public string? CaseNumber { get; set; }
    public string? CaseNotes { get; set; }
    public PartyRole Role { get; set; }
    public string? ClientRequests { get; set; }
    public DateOnly? EstimatedReviewDate { get; set; }
    public CaseStatus Status { get; set; }
    public string? LawyerOpinion { get; set; }
    public string AssignedOfficer { get; set; } = null!;
    public CourtType CourtType { get; set; }

    public ClientFileDto ClientFile { get; set; }
    public List<ClientDto> Clients { get; set; } = new();
}
