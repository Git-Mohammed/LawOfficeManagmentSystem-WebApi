using LOMs.Domain.Common.Results;

namespace LOMs.Domain.People.Phones;

public static class PhoneErrors
{
    public static readonly Error CodeRequired =
        Error.Validation("Phone.CodeRequired", "Phone code cannot be empty.");

    public static readonly Error CodeInvalid =
        Error.Validation("Phone.CodeInvalid", "Phone code must be 1–4 digits and cannot start with '+' or '00'.");

    public static readonly Error NumberRequired =
        Error.Validation("Phone.NumberRequired", "Phone number cannot be empty.");

    public static readonly Error NumberInvalid =
        Error.Validation("Phone.NumberInvalid", "Phone number must be 7–15 digits and may optionally start with '+'.");

    public static readonly Error NumberTooShort =
        Error.Validation("Phone.NumberTooShort", "Phone number must be at least 7 digits long.");

    public static readonly Error NumberTooLong =
        Error.Validation("Phone.NumberTooLong", "Phone number cannot exceed 15 digits.");

    public static readonly Error AlreadyExists =
        Error.Conflict("Phone.AlreadyExists", "A phone with this code and number already exists.");
}
