using LOMs.Domain.Common;
using LOMs.Domain.Common.Results;
using LOMs.Domain.People.Employees;

namespace LOMs.Domain.Cases.CourtSessions;

public sealed class CourtSession : AuditableEntity
{
    public Guid CaseId { get; private set; }
    public Guid AssignedEmployeeId { get; private set; }
    public DateTimeOffset ScheduledAt { get; private set; }
    public string AssignedTasks { get; private set; } = string.Empty;

    public bool IsSessionDateExpired => ScheduledAt < DateTimeOffset.UtcNow;

    // Navigation properties
    public Case? Case { get; set; }
    public Employee? AssignedEmployee { get; set; }

    private CourtSession() { }

    private CourtSession(Guid id, Guid caseId, Guid assignedEmployeeId, DateTimeOffset scheduledAt, string assignedTasks)
        : base(id)
    {
        CaseId = caseId;
        AssignedEmployeeId = assignedEmployeeId;
        ScheduledAt = scheduledAt;
        AssignedTasks = assignedTasks;
    }

    public static Result<CourtSession> Create(Guid id, Guid caseId, Guid assignedEmployeeId, DateTimeOffset scheduledAt, string assignedTasks)
    {
        if (id == Guid.Empty)
            return CourtSessionErrors.InvalidId;

        if (caseId == Guid.Empty)
            return CourtSessionErrors.MissingCaseId;

        if (assignedEmployeeId == Guid.Empty)
            return CourtSessionErrors.MissingEmployeeId;

        if (scheduledAt <= DateTimeOffset.UtcNow)
            return CourtSessionErrors.InvalidScheduledDate;

        if (string.IsNullOrWhiteSpace(assignedTasks))
            return CourtSessionErrors.MissingAssignedTasks;

        var session = new CourtSession(id, caseId, assignedEmployeeId, scheduledAt, assignedTasks.Trim());

        return session;
    }

    public Result<Success> Reschedule(DateTimeOffset newDate)
    {
        if (IsSessionDateExpired)
            return CourtSessionErrors.CannotRescheduleExpiredSession;

        if (newDate <= DateTimeOffset.UtcNow)
            return CourtSessionErrors.InvalidScheduledDate;

        ScheduledAt = newDate;
        return Result.Success;
    }

    public Result<Updated> Update(Guid caseId, Guid assignedEmployeeId, DateTimeOffset newDate, string assignedTasks)
    {
        if (IsSessionDateExpired)
            return CourtSessionErrors.CannotUpdateExpiredSession;

        if (caseId == Guid.Empty)
            return CourtSessionErrors.MissingCaseId;

        if (assignedEmployeeId == Guid.Empty)
            return CourtSessionErrors.MissingEmployeeId;

        if (newDate <= DateTimeOffset.UtcNow)
            return CourtSessionErrors.InvalidScheduledDate;

        if (string.IsNullOrWhiteSpace(assignedTasks))
            return CourtSessionErrors.MissingAssignedTasks;

        CaseId = caseId;
        AssignedEmployeeId = assignedEmployeeId;
        ScheduledAt = newDate;
        AssignedTasks = assignedTasks.Trim();

        return Result.Updated;
    }
}
