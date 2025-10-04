using LOMs.Domain.Common.Results;

namespace LOMs.Domain.Cases.Judgments;

public static class JudgmentErrors
{

    public static readonly Error InvalidId =
        Error.NotFound("Judgment.InvalidId", "Judgment not found or does not belong to the specified case.");

    public static readonly Error MissingFilePath =
        Error.Conflict("Judgment.MissingFilePath", "File path is required to download the judgment file.");

    public static readonly Error FileNotFound =
        Error.NotFound("Judgment.FileNotFound", "The requested judgment file could not be found.");

    public static readonly Error FileDownloadFailed =
        Error.Failure("Judgment.FileDownloadFailed", "An unexpected error occurred while downloading the judgment file.");

    public static readonly Error CaseIdRequired =
        Error.Validation("Judgment.CaseIdRequired", "CaseId is required.");

    public static readonly Error NumberRequired =
        Error.Validation("Judgment.NumberRequired", "Judgment number is required.");

    public static readonly Error InvalidNumber =
        Error.Validation("Judgment.InvalidNumber", "Judgment number is invalid.");

    public static readonly Error TypeRequired =
        Error.Validation("Judgment.TypeRequired", "Judgment type is required.");

    public static readonly Error SubTypeRequired =
        Error.Validation("Judgment.SubTypeRequired", "Judgment subtype is required.");

    public static readonly Error IssuedOnRequired =
        Error.Validation("Judgment.IssuedOnRequired", "IssuedOn date is required.");

    public static readonly Error ExpiresOnRequired =
        Error.Validation("Judgment.ExpiresOnRequired", "ExpiresOn date is required.");

    public static readonly Error InvalidDateRange =
        Error.Validation("Judgment.InvalidDateRange", "ExpiresOn must be after IssuedOn.");
    public static readonly Error NotFound =
        Error.NotFound("Judgment.NotFound", "Judgment not found.");
}