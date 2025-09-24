namespace LOMs.Application.Features.CourtSessions.Dtos;

public sealed class GetFutureCourtSessionDto
{
    public Guid CourtSessionId { get; init; }
    public Guid CaseId { get; init; }
    public Guid AssignedEmployeeId { get; init; }
    public string AssignedEmployeeName { get; init; } = string.Empty;
    public DateTimeOffset ScheduledAt { get; init; }
    public Guid CreatedByEmployeeId { get; init; }
    public string CreatedByEmployeeName { get; init; } = string.Empty;
    public DateTimeOffset CreatedDate { get; init; }
}
