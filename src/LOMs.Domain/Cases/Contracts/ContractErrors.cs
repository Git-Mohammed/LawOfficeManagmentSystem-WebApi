using LOMs.Domain.Common.Results;

namespace LOMs.Domain.Cases.Contracts;

public static class ContractErrors
{
    // 🔹 General Validation Errors
    public static readonly Error InvalidId =
        Error.Validation("Contract.InvalidId", "معرّف العقد لا يمكن أن يكون فارغًا.");

    public static readonly Error InvalidCaseId =
        Error.Validation("Contract.InvalidCaseId", "معرّف القضية لا يمكن أن يكون فارغًا.");

    public static readonly Error MissingContractNumber =
        Error.Validation("Contract.MissingContractNumber", "رقم العقد مطلوب.");

    public static readonly Error InvalidContractType =
        Error.Validation("Contract.InvalidContractType", "نوع العقد غير صالح أو غير معروف.");

    public static readonly Error InvalidCourtType =
        Error.Validation("Contract.InvalidCourtType", "نوع المحكمة غير صالح أو غير معروف.");

    public static readonly Error MissingFilePath =
        Error.Validation("Contract.MissingFilePath", "مرفق العقد مطلوب.");

    public static readonly Error InvalidBaseAmount =
        Error.Validation("Contract.InvalidBaseAmount", "قيمة العقد يجب أن تكون أكبر من أو تساوي صفر.");

    public static readonly Error InvalidInitialPayment =
        Error.Validation("Contract.InvalidInitialPayment", "مقدم العقد يجب أن يكون أكبر من أو يساوي صفر.");

    // 🔹 CreateContract-Specific Errors
    public static Error DuplicateContractNumber(string contractNumber) =>
        Error.Validation("CreateContract.DuplicateContractNumber", $"رقم العقد {contractNumber} موجود بالفعل.");

    public static readonly Error UnexpectedFailure =
        Error.Unexpected("CreateContract.Failure", "حدث خطأ غير متوقع أثناء إنشاء العقد.");
}
