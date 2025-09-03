using LOMs.Domain.Common.Results;

namespace LOMs.Domain.People;

public static class PersonErrors
{
    public static Error FullNameRequired =>
        Error.Validation("Person_FullName_Required", "الاسم الكامل مطلوب ويجب أن لا يقل عن 3 أحرف.");

    public static Error InvalidNationalId =>

        Error.Validation("Person_NationalId_Invalid", "رقم الهوية الوطنية يجب أن يتكون من 10 أرقام بالضبط.");

        Error.Validation("Person_NationalId_Invalid", "National ID must be exactly 11 digits.");
    public static Error ExistingNationalId =>
        Error.Validation("Person_Existing_NationalId", "National ID already exists.");


    public static Error InvalidBirthDate =>
        Error.Validation("Person_BirthDate_Invalid", "يجب أن يكون عمر الشخص 18 سنة على الأقل.");

    public static Error PhoneNumberRequired =>
        Error.Validation("Person_PhoneNumber_Required", "رقم الهاتف مطلوب.");

    public static Error InvalidPhoneNumber =>
        Error.Conflict("Person_PhoneNumber_Invalid", "رقم الهاتف يجب أن يتكون من 7 إلى 15 رقمًا وقد يبدأ بـ '+'.");

    public static Error AddressRequired =>
        Error.Validation("Person_Address_Required", "العنوان مطلوب ويجب أن يحتوي على تفاصيل كافية.");

    public static Error PersonAlreadyExists =>
        Error.Conflict("Person_Already_Exists", "يوجد شخص بنفس رقم الهوية الوطنية.");

    public static Error InvalidAddress =>
        Error.Validation("Person_Address_Invalid", "العنوان يجب أن لا يقل عن 10 أحرف ويحتوي على تفاصيل كافية.");

    public static Error PhoneNumberAlreadyExists =>
        Error.Conflict("Person_PhoneNumber_AlreadyExists", "يوجد شخص بنفس رقم الهاتف.");
}
