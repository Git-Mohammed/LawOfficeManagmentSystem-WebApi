using LOMs.Domain.Common.Results;

namespace LOMs.Domain.Cases.CourtTypes;

public static class CourtTypeErrors
{
    public static readonly Error InvalidId =
        Error.Validation("CourtType.InvalidId", "Court type ID cannot be empty.");

    public static readonly Error MissingName =
        Error.Validation("CourtType.MissingName", "Court type name is required.");

    public static readonly Error InvalidCode =
        Error.Validation("CourtType.InvalidCode", "Court type code must be greater than zero.");

    public static readonly Error FailedToRetrieveCourtTypes =
        Error.Unexpected("CourtType.FailedToRetrieveCourtTypes", "An error occurred while retrieving court types.");

    public static readonly Error NotFound =
        Error.NotFound("CourtType.NotFound", "Court type not found.");

    public static readonly Error FailedToRetrieveYears =
        Error.Unexpected("CourtType.FailedToRetrieveYears", "An error occurred while retrieving Hijri years associated with the court type.");

        public static readonly Error NoCourtTypesFound =
    Error.NotFound("CourtType.NoneFound", "No court types containing client files were found.");

}
