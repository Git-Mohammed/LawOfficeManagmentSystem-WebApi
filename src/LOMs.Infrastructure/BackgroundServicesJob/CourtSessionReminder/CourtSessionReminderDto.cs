namespace LOMs.Infrastructure.BackgroundServicesJob.CourtSessionReminder;

public sealed class CourtSessionReminderDto
{
    public Guid SessionId { get; init; }
    public DateTimeOffset ScheduledAt { get; init; }
    public string AssignedTasks { get; init; } = string.Empty;

    public Guid AssignedEmployeeId { get; init; }
    public string AssignedEmployeeEmail { get; init; } = string.Empty;
    public string AssignedEmployeeFullName { get; init; } = string.Empty;
}
