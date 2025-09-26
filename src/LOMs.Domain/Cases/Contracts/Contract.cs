using LOMs.Domain.Cases.CourtTypes;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Common;
using LOMs.Domain.Common.Results;

namespace LOMs.Domain.Cases.Contracts;

public sealed class Contract : AuditableEntity
{
    // Core identifiers
    public Guid CaseId { get; private set; }
    public short HijriYear { get; private set; }
    public short CourtTypeCode { get; private set; }

    // Contract details
    public ContractType Type { get; private set; }
    public DateOnly? IssuedOn { get; private set; }
    public DateOnly? ExpiresOn { get; private set; }

    // Financial details
    public decimal BaseAmount { get; private set; }
    public decimal InitialPayment { get; private set; }
    public decimal TaxAmount { get; private set; }
    public decimal TotalAmount => BaseAmount + TaxAmount;

    // Status
    public bool IsAssigned { get; private set; }

    // Navigation
    public Case? Case { get; private set; }
    private readonly List<ContractFile> _contractFiles = new();
    public IReadOnlyList<ContractFile> ContractFiles => _contractFiles.AsReadOnly();

    private Contract() { } // EF Core

    private Contract(
        Guid id,
        Guid caseId,
        ContractType type,
        DateOnly? issuedOn,
        DateOnly? expiresOn,
        decimal baseAmount,
        decimal initialPayment,
        decimal taxAmount,
        bool isAssigned,
        short courtTypeCode,
        short hijriYear
    ) : base(id)
    {
        CaseId = caseId;
        Type = type;
        IssuedOn = issuedOn;
        ExpiresOn = expiresOn;
        BaseAmount = baseAmount;
        InitialPayment = initialPayment;
        TaxAmount = taxAmount;
        IsAssigned = isAssigned;
        CourtTypeCode = courtTypeCode;
        HijriYear = hijriYear;
    }

    public static Result<Contract> Create(
        Guid caseId,
        CourtType courtType,
        ContractType type,
        DateOnly? issuedOn,
        DateOnly? expiresOn,
        decimal baseAmount,
        decimal initialPayment,
        bool isAssigned,
        short hijriYear
    )
    {
        if (caseId == Guid.Empty)
            return ContractErrors.InvalidCaseId;

        if (courtType is null)
            return ContractErrors.InvalidCourtType;

        if (!Enum.IsDefined(typeof(ContractType), type))
            return ContractErrors.InvalidContractType;

        if (type == ContractType.FixedTerm)
        {
            if (!issuedOn.HasValue)
                return ContractErrors.MissingIssueDate;

            if (issuedOn.Value < new DateOnly(1900, 1, 1))
                return ContractErrors.InvalidIssueDate;

            if (!expiresOn.HasValue)
                return ContractErrors.MissingExpiryDate;

            if (issuedOn > expiresOn)
                return ContractErrors.InvalidDateRange;
        }
        else
        {
            issuedOn = null;
            expiresOn = null;
        }

        if (baseAmount < 0)
            return ContractErrors.InvalidBaseAmount;

        if (initialPayment < 0)
            return ContractErrors.InvalidInitialPayment;

        var taxAmount = CalculateTax(baseAmount);

        var contract = new Contract(
            Guid.NewGuid(),
            caseId,
            type,
            issuedOn,
            expiresOn,
            baseAmount,
            initialPayment,
            taxAmount,
            isAssigned,
            courtType.Code,
            hijriYear
        );

        return contract;
    }

    private static decimal CalculateTax(decimal amount) =>
        Math.Round(amount * LOMs.Domain.Common.Constants.LOMsConstants.TaxRate, 2);
}
