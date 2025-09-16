using LOMs.Domain.Common.Results;

namespace LOMs.Domain.Cases.CourtSessions;

public static class CourtSessionErrors
{
    public static readonly Error InvalidId =
        Error.Validation("CourtSession.InvalidId", "Session ID must not be empty.");

    public static readonly Error MissingCaseId =
        Error.Validation("CourtSession.MissingCaseId", "Case ID is required.");

    public static readonly Error MissingEmployeeId =
        Error.Validation("CourtSession.MissingEmployeeId", "Assigned employee ID is required.");

    public static readonly Error InvalidScheduledDate =
        Error.Validation("CourtSession.InvalidScheduledDate", "Scheduled date must be in the future.");

    public static readonly Error MissingAssignedTasks =
        Error.Validation("CourtSession.MissingAssignedTasks", "Assigned tasks must not be empty.");

    public static readonly Error CannotUpdateExpiredSession =
    Error.Validation("CourtSession.CannotUpdateExpiredSession", "Cannot update a session that has already expired.");

    public static readonly Error CannotRescheduleExpiredSession =
        Error.Validation("CourtSession.CannotRescheduleExpiredSession", "Cannot reschedule a session that has already expired.");
    public static readonly Error EmployeeNotFound =
    Error.NotFound("CourtSession.EmployeeNotFound", "Assigned employee does not exist.");

    public static readonly Error CaseNotFound =
        Error.NotFound("CourtSession.CaseNotFound", "Case does not exist.");

}
