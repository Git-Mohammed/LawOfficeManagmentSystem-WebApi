using LOMs.Domain.Common.Results;

namespace LOMs.Domain.Cases;

public static class CaseErrors
{
    // 🔹 General Validation Errors
    public static Error Invalid_ClientId(string? id = null) =>
        Error.Validation("Case_Invalid_ClientId", id is null
            ? "معرّف العميل لا يمكن أن يكون فارغًا."
            : $"معرّف العميل غير صالح: {id}");

    public static Error Invalid_ClientFileId(string? id = null) =>
        Error.Validation("Case_Invalid_ClientFileId", id is null
            ? "معرّف ملف العميل لا يمكن أن يكون فارغًا."
            : $"معرّف ملف العميل غير صالح: {id}");

    public static readonly Error Invalid_CaseNotes =
        Error.Validation("Case_Invalid_CaseNotes", "ملاحظات القضية مطلوبة.");

    public static readonly Error Invalid_ClientRequests =
        Error.Validation("Case_Invalid_ClientRequests", "طلبات العميل يجب أن تكون محددة.");

    public static readonly Error Missing_AssignedOfficer =
        Error.Validation("Case_Missing_AssignedOfficer", "يجب تحديد الموظف المسؤول عن القضية.");

    public static readonly Error Invalid_Status =
        Error.Validation("Case_Invalid_Status", "حالة القضية غير صالحة أو غير معروفة.");

    public static readonly Error Invalid_ReviewDate =
        Error.Validation("Case_Invalid_ReviewDate", "تاريخ مراجعة القضية يجب أن يكون تاريخًا مستقبليًا صالحًا.");

    public static readonly Error Invalid_CaseNumber_Format =
        Error.Validation("Case_Invalid_CaseNumber_Format", "تنسيق رقم القضية غير صحيح. يرجى اتباع النمط المطلوب.");

    public static readonly Error Invalid_CourtType =
        Error.Validation("Case_Invalid_CourtType", "نوع المحكمة غير صالح أو غير معروف.");

    // 🔹 CreateCase-Specific Errors
    public static readonly Error Invalid_Clients =
        Error.Validation("CreateCase_InvalidClients", "يجب توفير عميل واحد على الأقل.");

    public static Error Duplicate_CaseNumber(string caseNumber) =>
        Error.Validation("CreateCase_DuplicateCaseNumber", $"رقم القضية {caseNumber} موجود بالفعل.");

    public static Error Duplicate_NationalIds(IEnumerable<string> nationalIds) =>
        Error.Validation("CreateCase_DuplicateNationalId", $"رقم/أرقام الهوية الوطنية موجودة بالفعل: {string.Join(", ", nationalIds)}");
    public static readonly Error Client_NotFound =
    Error.NotFound("CreateCase_ClientNotFound", $"العميل ليس موجوداً .");

    public static Error Client_NotFoundWithId(Guid clientId) =>
        Error.NotFound("CreateCase_ClientNotFoundWithId", $"لم يتم العثور على العميل بالمعرّف: {clientId}");

    public static Error Client_NotFoundWithIds(IEnumerable<Guid> clientIds) =>
    Error.NotFound("CreateCase_ClientNotFoundWithId", $"لم يتم العثور على العملاء بالمعرّف: {string.Join(",", clientIds)}");


    public static readonly Error Unknown_ClientType =
        Error.Unexpected("CreateCase_UnknownClientType", "نوع العميل غير معروف.");

    public static readonly Error Unexpected_Failure =
        Error.Unexpected("CreateCase_Failure", "حدث خطأ غير متوقع أثناء إنشاء القضية.");
}
