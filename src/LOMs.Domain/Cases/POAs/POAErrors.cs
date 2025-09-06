using LOMs.Domain.Common.Results;

namespace LOMs.Domain.POAs
{
    public static class POAErrors
    {
        // ─────────────────────────────
        // 🔹 أخطاء التحقق العامة
        // ─────────────────────────────

        public static Error Invalid_CaseId(Guid? caseId = null) =>
            Error.Validation(
                "POA_Invalid_CaseId",
                caseId is null || caseId == Guid.Empty
                    ? "معرّف القضية لا يمكن أن يكون فارغًا."
                    : $"معرّف القضية غير صالح: {caseId}"
            );

        public static Error Invalid_Number(string? number = null) =>
            Error.Validation(
                "POA_Invalid_Number",
                string.IsNullOrWhiteSpace(number)
                    ? "رقم الوكالة مطلوب ولا يمكن أن يكون فارغًا."
                    : $"رقم الوكالة غير صالح: {number}"
            );

        public static Error Invalid_IssueDate(DateOnly? date = null) =>
            Error.Validation(
                "POA_Invalid_IssueDate",
                date is null
                    ? "تاريخ إصدار الوكالة مطلوب."
                    : $"تاريخ الإصدار غير صالح: {date}"
            );

        public static Error Invalid_IssuingAuthority(string? authority = null) =>
            Error.Validation(
                "POA_Invalid_IssuingAuthority",
                string.IsNullOrWhiteSpace(authority)
                    ? "جهة إصدار الوكالة مطلوبة."
                    : $"جهة الإصدار غير صالحة: {authority}"
            );

        public static Error Invalid_AttachmentPath(string? path = null) =>
            Error.Validation(
                "POA_Invalid_AttachmentPath",
                string.IsNullOrWhiteSpace(path)
                    ? "مسار المرفق لا يمكن أن يكون فارغًا."
                    : $"مسار المرفق غير صالح: {path}"
            );

        // ─────────────────────────────
        // 🔹 أخطاء التحقق الإلزامية
        // ─────────────────────────────

        public static readonly Error Missing_Number =
            Error.Validation("POA_Missing_Number", "رقم الوكالة مطلوب ولا يمكن أن يكون فارغًا.");

        public static readonly Error Missing_IssuingAuthority =
            Error.Validation("POA_Missing_IssuingAuthority", "جهة إصدار الوكالة مطلوبة.");

        public static readonly Error Missing_Attachment =
            Error.Validation("POA_Missing_Attachment", "مرفق الوكالة مطلوب ولا يمكن أن يكون فارغًا.");

        // ─────────────────────────────
        // 🔹 أخطاء الإنشاء والاستعلام
        // ─────────────────────────────

        public static Error Duplicate_Number(string number) =>
            Error.Validation("POA_Duplicate_Number", $"رقم الوكالة {number} موجود بالفعل لهذه القضية.");

        public static readonly Error NotFound =
            Error.NotFound("POA_NotFound", "لم يتم العثور على الوكالة المطلوبة.");

        public static readonly Error Missing_CaseReference =
            Error.Validation("POA_Missing_CaseReference", "يجب ربط الوكالة بقضية صالحة.");

        public static readonly Error Creation_Failed =
            Error.Unexpected("POA_CreationFailed", "حدث خطأ غير متوقع أثناء إنشاء الوكالة.");
    }
}
