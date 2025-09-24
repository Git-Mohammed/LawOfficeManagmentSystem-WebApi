using LOMs.Domain.Common.Results;

namespace LOMs.Domain.People;

public static class PersonErrors
{
    public static readonly Error FullNameRequired =
        Error.Validation("Person.FullNameRequired", "Full name is required and must be at least 3 characters.");

    public static readonly Error InvalidNationalId =
        Error.Validation("Person.InvalidNationalId", "National ID must be exactly 10 digits.");

    public static readonly Error ExistingNationalId =
        Error.Conflict("Person.ExistingNationalId", "National ID already exists.");

    public static readonly Error CountryCodeRequired =
        Error.Validation("Person.CountryCodeRequired", "Country code is required.");

    public static readonly Error CountryCodeInvalid =
        Error.Validation("Person.CountryCodeInvalid", "Invalid country code (must be ISO Alpha-2).");

    public static readonly Error InvalidBirthDate =
        Error.Validation("Person.InvalidBirthDate", "Person must be at least 18 years old.");

    public static readonly Error PhoneNumberRequired =
        Error.Validation("Person.PhoneNumberRequired", "Phone number is required.");

    public static readonly Error InvalidPhoneNumber =
        Error.Validation("Person.InvalidPhoneNumber", "Phone number must be 7 to 15 digits and may start with '+'.");

    public static readonly Error PhoneCodeRequired =
        Error.Validation("Person.PhoneCodeRequired", "Phone country code is required.");

    public static readonly Error AddressRequired =
        Error.Validation("Person.AddressRequired", "Address is required and must contain sufficient detail.");

    public static readonly Error InvalidAddress =
        Error.Validation("Person.InvalidAddress", "Address must be at least 10 characters and contain sufficient detail.");

    public static readonly Error PersonAlreadyExists =
        Error.Conflict("Person.AlreadyExists", "A person with this national ID already exists.");

    public static readonly Error PhoneNumberAlreadyExists =
        Error.Conflict("Person.PhoneNumberAlreadyExists", "A person with this phone number already exists.");

    public static readonly Error AlreadyAssignedAsEmployee =
        Error.Conflict("Person.AlreadyAssignedAsEmployee", "This person is already assigned as an employee.");

    public static readonly Error AlreadyAssignedAsClient =
        Error.Conflict("Person.AlreadyAssignedAsClient", "This person is already assigned as a client.");

    public static readonly Error RoleAssignmentConflict =
        Error.Conflict("Person.RoleAssignmentConflict", "Cannot assign this role because the person is already linked to a conflicting role.");
}
