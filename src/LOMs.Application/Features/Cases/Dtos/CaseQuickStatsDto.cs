using LOMs.Domain.Cases.Enums;

namespace LOMs.Application.Features.Cases.Dtos;

public sealed class CaseQuickStatsDto
{
    public Guid CaseId { get; init; }
    public string FileNumber { get; set; } = default!;
    public string ClientName { get; set; } = default!;
    public string? CaseNumber { get; set; } = default!;
    public CaseStatus Status { get; set; } = default!;
}
