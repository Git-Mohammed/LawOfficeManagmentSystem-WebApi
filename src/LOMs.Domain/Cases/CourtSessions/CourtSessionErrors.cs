using LOMs.Domain.Common.Results;

namespace LOMs.Domain.Cases.CourtSessions;

public static class CourtSessionErrors
{
    // Validation Errors
    public static readonly Error InvalidId = Error.Validation(
        code: "CourtSession.InvalidId",
        description: "Session ID must not be empty.");

    public static readonly Error MissingCaseId = Error.Validation(
        code: "CourtSession.MissingCaseId",
        description: "Case ID is required.");

    public static readonly Error MissingEmployeeId = Error.Validation(
        code: "CourtSession.MissingEmployeeId",
        description: "Assigned employee ID is required.");

    public static readonly Error InvalidScheduledDate = Error.Validation(
        code: "CourtSession.InvalidScheduledDate",
        description: "Scheduled date must be in the future.");

    public static readonly Error MissingAssignedTasks = Error.Validation(
        code: "CourtSession.MissingAssignedTasks",
        description: "Assigned tasks must not be empty.");

    public static readonly Error CannotUpdateExpiredSession = Error.Validation(
        code: "CourtSession.CannotUpdateExpiredSession",
        description: "Cannot update a session that has already expired.");

    public static readonly Error CannotRescheduleExpiredSession = Error.Validation(
        code: "CourtSession.CannotRescheduleExpiredSession",
        description: "Cannot reschedule a session that has already expired.");

    // Not Found Errors
    public static readonly Error EmployeeNotFound = Error.NotFound(
        code: "CourtSession.EmployeeNotFound",
        description: "Assigned employee does not exist.");

    public static readonly Error CaseNotFound = Error.NotFound(
        code: "CourtSession.CaseNotFound",
        description: "Case does not exist.");

    public static readonly Error SessionNotFound = Error.NotFound(
        code: "CourtSession.SessionNotFound",
        description: "Court session does not exist for the specified case.");

    // Conflict Errors
    public static readonly Error EmployeeScheduleConflict = Error.Conflict(
        code: "CourtSession.EmployeeScheduleConflict",
        description: "The assigned employee is already scheduled for another court session at the same time.");
}
