using LOMs.Domain.Common.Results;

namespace LOMs.Domain.People;

public static class PersonErrors
{
    public static Error FullNameRequired =>
        Error.Validation("Person_FullName_Required", "Full name is required and must be at least 3 characters.");

    public static Error InvalidNationalId =>
        Error.Validation("Person_NationalId_Invalid", "National ID must be exactly 11 digits.");
    public static Error ExistingNationalId =>
        Error.Validation("Person_Existing_NationalId", "National ID already exists.");

    public static Error InvalidBirthDate =>
        Error.Validation("Person_BirthDate_Invalid", "Person must be at least 18 years old.");

    public static Error PhoneNumberRequired =>
        Error.Validation("Person_PhoneNumber_Required", "Phone number is required.");

    public static Error InvalidPhoneNumber =>
        Error.Conflict("Person_PhoneNumber_Invalid", "Phone number must be 7–15 digits and may start with '+'.");

    public static Error AddressRequired =>
        Error.Validation("Person_Address_Required", "Address is required and must contain sufficient details.");

    public static Error PersonAlreadyExists =>
        Error.Conflict("Person_Already_Exists", "A person with this national ID already exists.");

    public static Error InvalidAddress =>
    Error.Validation("Person_Address_Invalid", "Address must be at least 10 characters and contain sufficient details.");

    public static Error PhoneNumberAlreadyExists =>
    Error.Conflict("Person_PhoneNumber_AlreadyExists", "A person with this phone number already exists.");

}
