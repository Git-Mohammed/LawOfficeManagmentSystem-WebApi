using LOMs.Domain.Common.Results;

namespace LOMs.Domain.Cases.Contracts
{
    public static class ContractErrors
    {
        // ─────────────────────────────
        // 🔹 General Validation Errors
        // ─────────────────────────────
        public static readonly Error Invalid_Id =
            Error.Validation("Contract_Invalid_Id", "معرّف العقد لا يمكن أن يكون فارغًا.");

        public static readonly Error Invalid_CaseId =
            Error.Validation("Contract_Invalid_CaseId", "معرّف القضية لا يمكن أن يكون فارغًا.");

        public static readonly Error Missing_ContractNumber =
            Error.Validation("Contract_Missing_ContractNumber", "رقم العقد مطلوب.");

        public static readonly Error Invalid_ContractType =
            Error.Validation("Contract_Invalid_ContractType", "نوع العقد غير صالح أو غير معروف.");

        public static readonly Error Invalid_CourtType =
            Error.Validation("Contract_Invalid_CourtType", "نوع المحكمة غير صالح أو غير معروف.");

        public static readonly Error Missing_FilePath =
            Error.Validation("Contract_Missing_FilePath", "مرفق العقد مطلوب.");

        public static readonly Error Invalid_TotalAmount =
            Error.Validation("Contract_Invalid_TotalAmount", "قيمة العقد يجب أن تكون أكبر من أو تساوي صفر.");

        public static readonly Error Invalid_InitialPayment =
            Error.Validation("Contract_Invalid_InitialPayment", "مقدم العقد يجب أن يكون أكبر من أو يساوي صفر.");

        // ─────────────────────────────
        // 🔹 CreateContract-Specific Errors
        // ─────────────────────────────
        public static Error Duplicate_ContractNumber(string contractNumber) =>
            Error.Validation("CreateContract_DuplicateContractNumber", $"رقم العقد {contractNumber} موجود بالفعل.");

        public static readonly Error Unexpected_Failure =
            Error.Unexpected("CreateContract_Failure", "حدث خطأ غير متوقع أثناء إنشاء العقد.");
    }
}
