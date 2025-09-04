namespace LOMs.Application.Features.Cases.Dtos;

public sealed class POADto
{
    public Guid Id { get; init; }                          // Unique identifier for the POA

    public Guid CaseId { get; init; }                      // Linked case ID

    public string Number { get; init; } = string.Empty;    // Official POA number

    public DateOnly IssueDate { get; init; }               // Date the POA was issued

    public string IssuingAuthority { get; init; } = string.Empty; // Authority that issued the POA

    public string AttachmentUrl { get; init; } = string.Empty;    // Public URL or path to the POA document
}
