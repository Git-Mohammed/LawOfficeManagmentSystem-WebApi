using LOMs.Domain.Common.Results;

namespace LOMs.Domain.Cases.ClientFiles;

public static class ClientFileErrors
{
    public static readonly Error InvalidId =
        Error.Validation("ClientFile.InvalidId", "Client file ID is invalid.");

    public static readonly Error InvalidClientId =
        Error.Validation("ClientFile.InvalidClientId", "Client ID is invalid.");

    public static readonly Error InvalidCourtType =
        Error.Validation("ClientFile.InvalidCourtType", "Court type is invalid.");

    public static readonly Error InvalidFileNumber =
        Error.Validation("ClientFile.InvalidFileNumber", "File number is invalid or empty.");

    public static readonly Error InvalidHijriYear =
        Error.Validation("ClientFile.InvalidHijriYear", "Hijri year is invalid.");

    public static readonly Error InvalidOrderNumber =
        Error.Validation("ClientFile.InvalidOrderNumber", "Order number is invalid.");
}
