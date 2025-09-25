using LOMs.Domain.Common.Results;

namespace LOMs.Domain.Cases.Contracts;

public static class ContractErrors
{
    // 🔹 General Validation Errors
    public static readonly Error InvalidId =
        Error.Validation("Contract.InvalidId", "Contract ID cannot be empty.");

    public static readonly Error InvalidCaseId =
        Error.Validation("Contract.InvalidCaseId", "Case ID cannot be empty.");

    public static readonly Error MissingContractNumber =
        Error.Validation("Contract.MissingContractNumber", "Contract number is required.");

    public static readonly Error InvalidContractType =
        Error.Validation("Contract.InvalidContractType", "Contract type is invalid or unknown.");

    public static readonly Error InvalidCourtType =
        Error.Validation("Contract.InvalidCourtType", "Court type is invalid or unknown.");

    public static readonly Error MissingFilePath =
        Error.Validation("Contract.MissingFilePath", "Contract attachment is required.");

    public static readonly Error InvalidBaseAmount =
        Error.Validation("Contract.InvalidBaseAmount", "Contract amount must be greater than or equal to zero.");

    public static readonly Error InvalidInitialPayment =
        Error.Validation("Contract.InvalidInitialPayment", "Initial payment must be greater than or equal to zero.");

    // 🔹 Fixed-Term Contract-Specific Errors
    public static readonly Error MissingIssueDate =
        Error.Validation("Contract.MissingIssueDate", "Issue date is required for fixed-term contracts.");

    public static readonly Error InvalidIssueDate =
        Error.Validation("Contract.InvalidIssueDate", "Issue date is invalid.");

    public static readonly Error MissingExpiryDate =
        Error.Validation("Contract.MissingExpiryDate", "Expiry date is required for fixed-term contracts.");

    public static readonly Error InvalidDateRange =
        Error.Validation("Contract.InvalidDateRange", "Issue date must be earlier than expiry date.");

    // 🔹 Retrieval Errors
    public static readonly Error ContractsNotFoundByCaseId =
        Error.NotFound("Contract.ContractsNotFoundByCaseId", "No contracts found for the specified case ID.");

    public static readonly Error ContractNotFound =
        Error.NotFound("Contract.NotFound", "The specified contract could not be found for the given case.");

    public static readonly Error FileNotFound =
        Error.NotFound("Contract.FileNotFound", "The specified file could not be found for the given contract.");

    public static readonly Error CaseNotFound =
        Error.NotFound("Contract.CaseNotFound", "The specified case could not be found.");

    // 🔹 CreateContract-Specific Errors
    public static Error DuplicateContractNumber(string contractNumber) =>
        Error.Validation("CreateContract.DuplicateContractNumber", $"Contract number {contractNumber} already exists.");

    public static readonly Error UnexpectedFailure =
        Error.Unexpected("CreateContract.Failure", "An unexpected error occurred while creating the contract.");
}
