using LOMs.Domain.Common.Results;

namespace LOMs.Domain.Cases
{
    public static class CaseErrors
    {
        public static readonly Error Invalid_ClientId =
            Error.Validation("Case_Invalid_ClientId", "معرّف العميل لا يمكن أن يكون فارغًا.");

        public static readonly Error Invalid_ClientFileId =
            Error.Validation("Case_Invalid_ClientFileId", "معرّف ملف العميل لا يمكن أن يكون فارغًا.");

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

    }
}
