namespace LOMs.Application.Features.CourtSessions.Dtos;

public sealed class CourtSessionDto
{
    public Guid CourtSessionId { get; init; }
    public Guid CaseId { get; init; }
    public Guid AssignedEmployeeId { get; init; }
    public string AssignedEmployeeName { get; init; } = string.Empty;
    public DateTimeOffset ScheduledAt { get; init; }
    public string AssignedTasks { get; init; } = string.Empty;
    public bool IsSessionDateExpired { get; init; }
}
