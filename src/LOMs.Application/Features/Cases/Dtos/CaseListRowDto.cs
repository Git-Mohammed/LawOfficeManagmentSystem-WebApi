using LOMs.Domain.Cases.Enums;

namespace LOMs.Application.Features.Cases.Dtos;

/// <summary>
/// Represents a single row in the paginated case list.
/// </summary>
public sealed class CaseListRowDto
{
    public Guid CaseId { get; init; }
    public string CaseNumber { get; init; } = default!;
    public string CaseSubject { get; init; } = default!;
    public string CourtTypeName { get; init; } = default!;
    public CaseStatus Status { get; init; } = default!;
    public string FileNumber { get; init; } = default!;
    public string CreatedByName { get; init; } = default!;
    public DateTimeOffset CreatedDate { get; init; } = default!;
}
