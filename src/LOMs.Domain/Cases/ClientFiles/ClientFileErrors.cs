using LOMs.Domain.Common.Results;

namespace LOMs.Domain.Cases.ClientFiles;

public static class ClientFileErrors
{
    public static readonly Error InvalidId =
        Error.Validation("ClientFile.InvalidId", "معرّف ملف العميل غير صالح.");

    public static readonly Error InvalidClientId =
        Error.Validation("ClientFile.InvalidClientId", "معرّف العميل غير صالح.");

    public static readonly Error InvalidCourtType =
        Error.Validation("ClientFile.InvalidCourtType", "نوع المحكمة غير صالح.");

    public static readonly Error InvalidFileNumber =
        Error.Validation("ClientFile.InvalidFileNumber", "رقم الملف غير صالح أو فارغ.");

    public static readonly Error InvalidHijriYear =
        Error.Validation("ClientFile.InvalidHijriYear", "سنة الهجري غير صالحة.");

    public static readonly Error InvalidOrderNumber =
        Error.Validation("ClientFile.InvalidOrderNumber", "رقم الأمر غير صالح.");
}
