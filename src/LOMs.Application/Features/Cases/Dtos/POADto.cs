namespace LOMs.Application.Features.Cases.Dtos;

public sealed class POADto
{
    public Guid POAId { get; init; }                          // Unique identifier for the POA


    public string POANumber { get; init; } = string.Empty;    // Official POA number

    public DateOnly IssueDate { get; init; }               // Date the POA was issued

    public string IssuingAuthority { get; init; } = string.Empty; // Authority that issued the POA

    public string AttachmentPath { get; init; } = string.Empty;    // Public URL or path to the POA document
}
