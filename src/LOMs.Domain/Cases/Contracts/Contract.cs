using LOMs.Domain.Cases.CourtTypes;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Common;
using LOMs.Domain.Common.Results;
using System.Globalization;

namespace LOMs.Domain.Cases.Contracts;

public sealed class Contract : AuditableEntity
{
    // TODO: 
    // 1- Asks About AmountTax
    public Guid CaseId { get; private set; }
    public string ContractNumber { get; private set; } = null!;
    public short HijriYear { get; private set; }
    public short CourtTypeCode { get; private set; }
    public int OrderNumber { get; private set; }
    public ContractType Type { get; private set; }
    public DateOnly? IssuedOn { get; private set; }
    public DateOnly? ExpiresOn { get; private set; }
    public decimal BaseAmount { get; private set; }
    public decimal InitialPayment { get; private set; }
    public decimal TaxAmount { get; private set; }
    public decimal TotalAmount => BaseAmount + TaxAmount;
    public bool IsAssigned { get; private set; }

    public Case? Case { get; private set; }
    private List<ContractFile> _contractFiles = [];
    public IReadOnlyList<ContractFile> ContractFiles  => _contractFiles.AsReadOnly();

    private Contract() { } // EF Core
    // شوف موضوع TaxAmount
    // client get by national id
    // get all court types
    private Contract(
        Guid id,
        Guid caseId,
        string contractNumber,
        ContractType type,
        DateOnly? issuedOn,
        DateOnly? expiresOn,
        decimal baseAmount,
        decimal initialPayment,
        decimal taxAmount,
        bool isAssigned,
        short courtTypeCode,
        short hijriYear,
        int orderNumber,
        List<ContractFile> contractFiles 
    ) : base(id)
    {
        CaseId = caseId;
        ContractNumber = contractNumber;
        Type = type;
        IssuedOn = issuedOn;
        ExpiresOn = expiresOn;
        BaseAmount = baseAmount;
        InitialPayment = initialPayment;
        TaxAmount = taxAmount;
        IsAssigned = isAssigned;
        CourtTypeCode = courtTypeCode;
        HijriYear = hijriYear;
        OrderNumber = orderNumber;
        _contractFiles = contractFiles;
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
        short hijriYear,
        int orderNumber,
        List<ContractFile> contractFiles = null!
    )
    {
        if (caseId == Guid.Empty)
            return ContractErrors.InvalidCaseId;

        if (courtType is null)
            return ContractErrors.InvalidCourtType;

        if (!Enum.IsDefined(typeof(ContractType), type))
            return ContractErrors.InvalidCourtType;

        if(type is ContractType.FixedTerm)
        {
            if (!issuedOn.HasValue)
                return ContractErrors.MissingIssueDate;
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

        if (contractFiles is null || contractFiles.Count <= 0)
        {
            return ContractErrors.MissingFilePath;
        }    

            var contractNumber = GenerateFileNumber(courtType, hijriYear, orderNumber);
        var taxAmount = CalculateTax(baseAmount);
        var contract = new Contract(
            Guid.NewGuid(),
            caseId,
            contractNumber,
            type,
            issuedOn,
            expiresOn,
            baseAmount,
            initialPayment,
            taxAmount,
            isAssigned,
            courtType.Code,
            hijriYear,
            orderNumber,
            contractFiles
        );

        return contract;
    }

    private static decimal CalculateTax(decimal amount) =>
        Math.Round(amount * LOMs.Domain.Common.Constants.LOMsConstants.TaxRate, 2);

    /// <summary>
    /// Generates FileNumber in format: عYY-CourtCode-OrderNumber
    /// </summary>
    private static string GenerateFileNumber(CourtType courtType, short hijriYear, int orderNumber)
    {
        return $"ع{(hijriYear % 100):D2}-{courtType.Code}-{orderNumber:D4}";
    }

    public override string ToString() => $"Contract #{ContractNumber}";
}
