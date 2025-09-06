using LOMs.Domain.Common.Results;

namespace LOMs.Domain.People;

public static class PersonErrors
{
    public static readonly Error FullNameRequired =
        Error.Validation("Person.FullNameRequired", "الاسم الكامل مطلوب ويجب أن لا يقل عن 3 أحرف.");

    public static readonly Error InvalidNationalId =
        Error.Validation("Person.InvalidNationalId", "رقم الهوية الوطنية يجب أن يتكون من 10 أرقام بالضبط.");

    public static readonly Error ExistingNationalId =
        Error.Conflict("Person.ExistingNationalId", "رقم الهوية الوطنية موجود بالفعل.");

    public static readonly Error CountryCodeRequired =
        Error.Validation("Person.CountryCodeRequired", "رمز الدولة مطلوب.");

    public static readonly Error CountryCodeInvalid =
        Error.Validation("Person.CountryCodeInvalid", "رمز الدولة غير صالح (يجب أن يكون ISO Alpha-2).");

    public static readonly Error InvalidBirthDate =
        Error.Validation("Person.InvalidBirthDate", "يجب أن يكون عمر الشخص 18 سنة على الأقل.");

    public static readonly Error PhoneNumberRequired =
        Error.Validation("Person.PhoneNumberRequired", "رقم الهاتف مطلوب.");

    public static readonly Error InvalidPhoneNumber =
        Error.Validation("Person.InvalidPhoneNumber", "رقم الهاتف يجب أن يتكون من 7 إلى 15 رقمًا وقد يبدأ بـ '+'.");

    public static readonly Error AddressRequired =
        Error.Validation("Person.AddressRequired", "العنوان مطلوب ويجب أن يحتوي على تفاصيل كافية.");

    public static readonly Error InvalidAddress =
        Error.Validation("Person.InvalidAddress", "العنوان يجب أن لا يقل عن 10 أحرف ويحتوي على تفاصيل كافية.");

    public static readonly Error PersonAlreadyExists =
        Error.Conflict("Person.AlreadyExists", "يوجد شخص بنفس رقم الهوية الوطنية.");

    public static readonly Error PhoneNumberAlreadyExists =
        Error.Conflict("Person.PhoneNumberAlreadyExists", "يوجد شخص بنفس رقم الهاتف.");
}
