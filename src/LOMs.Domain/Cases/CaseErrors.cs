using LOMs.Domain.Common.Results;

namespace LOMs.Domain.Cases;

public static class CaseErrors
{
    // ─────────────────────────────
    // 🔹 General Validation Errors
    // ─────────────────────────────

    public static readonly Error CaseNotFound =
        Error.NotFound("Case.NotFound", "The requested case was not found.");

    public static readonly Error InvalidCaseId =
        Error.Validation("Case.InvalidCaseId", "Case ID cannot be empty.");

    public static Error InvalidClientId(Guid? id = null) =>
        Error.Validation(
            "Case.InvalidClientId",
            id is null
                ? "Client ID cannot be empty."
                : $"Invalid client ID: {id}"
        );

    public static Error InvalidCourtTypeId(Guid? id = null) =>
        Error.Validation(
            "Case.InvalidCourtTypeId",
            id is null
                ? "Court type ID cannot be empty."
                : $"Invalid court type ID: {id}"
        );

    public static Error DuplicatePhoneNumbers(HashSet<string> phones) =>
        Error.Conflict("Case.DuplicatePhoneNumbers", $"Duplicate phone numbers detected: {string.Join(", ", phones)}");

    public static readonly Error InvalidCaseSubject =
        Error.Validation("Case.InvalidCaseSubject", "Case subject is required.");

    public static readonly Error InvalidClientRequests =
        Error.Validation("Case.InvalidClientRequests", "Client requests must be specified.");

    public static readonly Error MissingAssignedEmployee =
        Error.Validation("Case.MissingAssignedOfficer", "Assigned employee must be specified.");

    public static readonly Error InvalidStatus =
        Error.Validation("Case.InvalidStatus", "Invalid or unknown case status.");

    public static readonly Error InvalidReviewDate =
        Error.Validation("Case.InvalidReviewDate", "Review date must be a valid future date.");

    public static readonly Error InvalidCaseNumberFormat =
        Error.Validation("Case.InvalidCaseNumberFormat", "Invalid case number format. Please follow the required pattern.");

    public static readonly Error InvalidCourtType =
        Error.Validation("Case.InvalidCourtType", "Invalid or unknown court type.");

    public static readonly Error InvalidPartyRole =
        Error.Validation("Case.InvalidPartyRole", "Invalid or unknown party role.");

    // ─────────────────────────────
    // 🔹 Employee Errors
    // ─────────────────────────────

    public static readonly Error EmptyEmployeeId =
        Error.Validation("Case.EmptyEmployeeId", "Employee ID cannot be empty.");

    public static Error AssignedEmployeeNotFound(Guid officerId) =>
        Error.NotFound("Case.AssignedEmployeeNotFound", $"Assigned employee not found with ID: {officerId}");

    // ─────────────────────────────
    // 🔹 CreateCase-Specific Errors
    // ─────────────────────────────

    public static readonly Error InvalidClients =
        Error.Validation("CreateCase.InvalidClients", "At least one client must be provided.");

    public static Error DuplicateCaseNumber(string caseNumber) =>
        Error.Validation("CreateCase.DuplicateCaseNumber", $"Case number '{caseNumber}' already exists.");

    public static Error DuplicateNationalId(Guid nationalId) =>
        Error.Validation("CreateCase.DuplicateNationalId", $"National ID already exists: {nationalId}");

    public static Error DuplicateNationalIds(IEnumerable<string> nationalIds) =>
        Error.Validation("CreateCase.DuplicateNationalIds", $"Duplicate national IDs detected: {string.Join(", ", nationalIds)}");

    // ✅ NEW ERROR
    public static readonly Error NationalIdRequired =
        Error.Validation("CreateCase.NationalIdRequired", "At least one national ID must be provided.");

    public static readonly Error ClientNotFound =
        Error.NotFound("CreateCase.ClientNotFound", "Client not found.");

    public static Error ClientNotFoundWithId(Guid clientId) =>
        Error.NotFound("CreateCase.ClientNotFoundWithId", $"Client not found with ID: {clientId}");

    public static Error ClientNotFoundWithIds(IEnumerable<Guid> clientIds) =>
        Error.NotFound("CreateCase.ClientNotFoundWithIds", $"Clients not found with IDs: {string.Join(", ", clientIds)}");

    public static readonly Error UnknownClientType =
        Error.Unexpected("CreateCase.UnknownClientType", "Unknown client type.");

    public static readonly Error UnexpectedFailure =
        Error.Unexpected("CreateCase.Failure", "An unexpected error occurred while creating the case.");

    // ─────────────────────────────
    // 🔹 File & Document Errors
    // ─────────────────────────────

    public static Error InvalidClientFileId(string? id = null) =>
        Error.Validation(
            "Case.InvalidClientFileId",
            id is null
                ? "Client file ID cannot be empty."
                : $"Invalid client file ID: {id}"
        );

    public static readonly Error ContractFilesMissing =
        Error.Validation("Case.ContractFilesMissing", "Contracts were indicated, but no contract files were attached.");

    public static readonly Error PoasMissing =
        Error.Validation("Case.PoasMissing", "POAs were indicated, but no POA files were attached.");

    public static readonly Error SaveContractFileFailure =
        Error.Unexpected("Case.SaveContractFileFailure", "An error occurred while saving the contract file.");

    public static readonly Error ImageSaveFailure =
        Error.Unexpected("Case.ImageSaveFailure", "An error occurred while saving the image file.");
}
