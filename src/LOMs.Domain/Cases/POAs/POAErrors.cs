using LOMs.Domain.Common.Results;

namespace LOMs.Domain.POAs;

public static class POAErrors
{
    public static Error InvalidCaseId(Guid? caseId = null) =>
        Error.Validation("POA.InvalidCaseId",
            caseId is null || caseId == Guid.Empty
                ? "Case ID cannot be empty."
                : $"Invalid Case ID: {caseId}");

    public static Error InvalidNumber(string? number = null) =>
        Error.Validation("POA.InvalidNumber",
            string.IsNullOrWhiteSpace(number)
                ? "POA number is required and cannot be empty."
                : $"Invalid POA number: {number}");

    public static Error InvalidIssueDate(DateOnly? date = null) =>
        Error.Validation("POA.InvalidIssueDate",
            date is null
                ? "Issue date is required."
                : $"Invalid issue date: {date}");

    public static Error InvalidIssuingAuthority(string? authority = null) =>
        Error.Validation("POA.InvalidIssuingAuthority",
            string.IsNullOrWhiteSpace(authority)
                ? "Issuing authority is required."
                : $"Invalid issuing authority: {authority}");

    public static Error InvalidAttachmentPath(string? path = null) =>
        Error.Validation("POA.InvalidAttachmentPath",
            string.IsNullOrWhiteSpace(path)
                ? "Attachment path cannot be empty."
                : $"Invalid attachment path: {path}");

    public static readonly Error MissingNumber =
        Error.Validation("POA.MissingNumber", "POA number is required and cannot be empty.");

    public static readonly Error MissingIssuingAuthority =
        Error.Validation("POA.MissingIssuingAuthority", "Issuing authority is required.");

    public static readonly Error MissingAttachment =
        Error.Validation("POA.MissingAttachment", "POA attachment is required and cannot be empty.");

    public static Error DuplicateNumber(string number) =>
        Error.Validation("POA.DuplicateNumber", $"POA number '{number}' already exists for this case.");

    public static readonly Error NotFound =
        Error.NotFound("POA.NotFound", "The requested POA was not found.");

    public static readonly Error MissingCaseReference =
        Error.Validation("POA.MissingCaseReference", "POA must be linked to a valid case.");

    public static readonly Error CreationFailed =
        Error.Unexpected("POA.CreationFailed", "An unexpected error occurred while creating the POA.");

    public static readonly Error MissingFilePath =
    Error.Conflict("POA.MissingFilePath", "File path is required to download the POA file.");

    public static readonly Error InvalidId =
        Error.NotFound("POA.InvalidId", "POA not found or does not belong to the specified case.");

    public static readonly Error FileNotFound =
        Error.NotFound("POA.FileNotFound", "The requested POA file could not be found.");

    public static readonly Error FileAccessDenied =
        Error.Forbidden("POA.FileAccessDenied", "You do not have permission to access this POA file.");

    public static readonly Error FileDownloadFailed =
        Error.Failure("POA.FileDownloadFailed", "An unexpected error occurred while downloading the POA file.");

}
