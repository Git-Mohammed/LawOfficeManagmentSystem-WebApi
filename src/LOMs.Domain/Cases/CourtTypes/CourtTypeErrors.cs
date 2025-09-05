using LOMs.Domain.Common.Results;

namespace LOMs.Domain.Cases.CourtTypes;

public static class CourtTypeErrors
{
    public static readonly Error InvalidId =
        Error.Validation("CourtType.InvalidId", "معرّف نوع المحكمة لا يمكن أن يكون فارغًا.");

    public static readonly Error MissingName =
        Error.Validation("CourtType.MissingName", "اسم نوع المحكمة مطلوب.");

    public static readonly Error InvalidCode =
        Error.Validation("CourtType.InvalidCode", "رمز نوع المحكمة يجب أن يكون أكبر من الصفر.");
    public static readonly Error FailedToRetrieveCourtTypes =
    Error.Unexpected("CourtType.FailedToRetrieveCourtTypes", "حدث خطأ أثناء جلب أنواع المحاكم.");
}
