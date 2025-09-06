using LOMs.Domain.Common.Results;

namespace LOMs.Domain.Cases;

public static class CaseErrors
{
    // ─────────────────────────────
    // 🔹 General Validation Errors
    // ─────────────────────────────

    public static readonly Error InvalidCaseId =
        Error.Validation("Case.InvalidCaseId", "معرّف القضية لا يمكن أن يكون فارغًا.");

    public static Error InvalidClientId(Guid? id = null) =>
        Error.Validation(
            "Case.InvalidClientId",
            id is null
                ? "معرّف العميل لا يمكن أن يكون فارغًا."
                : $"معرّف العميل غير صالح: {id}"
        );

    public static Error InvalidCourtTypeId(Guid? id = null) =>
        Error.Validation(
            "Case.InvalidCourtTypeId",
            id is null
                ? "معرّف نوع المحكمة لا يمكن أن يكون فارغًا."
                : $"معرّف نوع المحكمة غير صالح: {id}"
        );

    public static readonly Error InvalidCaseSubject =
        Error.Validation("Case.InvalidCaseSubject", "موضوع القضية مطلوب.");

    public static readonly Error InvalidClientRequests =
        Error.Validation("Case.InvalidClientRequests", "طلبات العميل يجب أن تكون محددة.");

    public static readonly Error MissingAssignedEmployee =
        Error.Validation("Case.MissingAssignedOfficer", "يجب تحديد الموظف المسؤول عن القضية.");

    public static readonly Error InvalidStatus =
        Error.Validation("Case.InvalidStatus", "حالة القضية غير صالحة أو غير معروفة.");

    public static readonly Error InvalidReviewDate =
        Error.Validation("Case.InvalidReviewDate", "تاريخ مراجعة القضية يجب أن يكون تاريخًا مستقبليًا صالحًا.");

    public static readonly Error InvalidCaseNumberFormat =
        Error.Validation("Case.InvalidCaseNumberFormat", "تنسيق رقم القضية غير صحيح. يرجى اتباع النمط المطلوب.");

    public static readonly Error InvalidCourtType =
        Error.Validation("Case.InvalidCourtType", "نوع المحكمة غير صالح أو غير معروف.");

    public static readonly Error InvalidPartyRole =
        Error.Validation("Case.InvalidPartyRole", "نوع الطرف غير صالح أو غير معروف.");

    // ─────────────────────────────
    // 🔹 Employee Errors
    // ─────────────────────────────

    public static readonly Error EmptyEmployeeId =
        Error.Validation("Case.EmptyEmployeeId", "معرّف الموظف لا يمكن أن يكون فارغًا.");

    public static Error AssignedEmployeeNotFound(Guid officerId) =>
        Error.NotFound("Case.AssignedEmployeeNotFound", $"لم يتم العثور على الموظف بالمعرّف: {officerId}");

    // ─────────────────────────────
    // 🔹 CreateCase-Specific Errors
    // ─────────────────────────────

    public static readonly Error InvalidClients =
        Error.Validation("CreateCase.InvalidClients", "يجب توفير عميل واحد على الأقل.");

    public static Error DuplicateCaseNumber(string caseNumber) =>
        Error.Validation("CreateCase.DuplicateCaseNumber", $"رقم القضية {caseNumber} موجود بالفعل.");
    public static Error DuplicateNationalId(Guid nationalId) =>
    Error.Validation("CreateCase.DuplicateNationalIds", $"رقم الهوية الوطنية موجودة بالفعل: {nationalId}");

    public static Error DuplicateNationalIds(IEnumerable<string> nationalIds) =>
        Error.Validation("CreateCase.DuplicateNationalIds", $"رقم/أرقام الهوية الوطنية موجودة بالفعل: {string.Join(", ", nationalIds)}");

    public static readonly Error ClientNotFound =
        Error.NotFound("CreateCase.ClientNotFound", "العميل غير موجود.");

    public static Error ClientNotFoundWithId(Guid clientId) =>
        Error.NotFound("CreateCase.ClientNotFoundWithId", $"لم يتم العثور على العميل بالمعرّف: {clientId}");

    public static Error ClientNotFoundWithIds(IEnumerable<Guid> clientIds) =>
        Error.NotFound("CreateCase.ClientNotFoundWithIds", $"لم يتم العثور على العملاء بالمعرّفات: {string.Join(", ", clientIds)}");

    public static readonly Error UnknownClientType =
        Error.Unexpected("CreateCase.UnknownClientType", "نوع العميل غير معروف.");

    public static readonly Error UnexpectedFailure =
        Error.Unexpected("CreateCase.Failure", "حدث خطأ غير متوقع أثناء إنشاء القضية.");

    // ─────────────────────────────
    // 🔹 File & Document Errors
    // ─────────────────────────────

    public static Error InvalidClientFileId(string? id = null) =>
        Error.Validation(
            "Case.InvalidClientFileId",
            id is null
                ? "معرّف ملف العميل لا يمكن أن يكون فارغًا."
                : $"معرّف ملف العميل غير صالح: {id}"
        );

    public static readonly Error ContractFilesMissing =
        Error.Validation("Case.ContractFilesMissing", "تم تحديد وجود عقود، ولكن لم يتم إرفاق أي ملفات عقود.");

    public static readonly Error PoasMissing =
        Error.Validation("Case.PoasMissing", "تم تحديد وجود توكيلات، ولكن لم يتم إرفاق أي ملفات توكيلات.");

    public static readonly Error SaveContractFileFailure =
        Error.Unexpected("Case.SaveContractFileFailure", "حدث خطأ أثناء حفظ ملف العقد.");

    public static readonly Error ImageSaveFailure =
        Error.Unexpected("Case.ImageSaveFailure", "حدث خطأ أثناء حفظ الملف.");
}
