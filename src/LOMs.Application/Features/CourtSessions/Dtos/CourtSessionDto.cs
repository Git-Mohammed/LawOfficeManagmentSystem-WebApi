using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.CourtSessions.Dtos;

public sealed class CourtSessionDto
{
    public Guid CourtSessionId { get; init; }
    public Guid AssignedEmployeeId { get; init; }
    public string AssignedEmployeeName { get; init; } = string.Empty;
    public DateTimeOffset ScheduledAt { get; init; }
    public string AssignedTasks { get; init; } = string.Empty;
    public DateTimeOffset CreatedAt { get; init; }
    public string EmployeeCreateItName {  get; init; } = string.Empty;
}
