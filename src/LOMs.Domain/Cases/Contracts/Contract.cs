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
    public string FilePath { get; private set; } = null!;
    public bool IsAssigned { get; private set; }

    public Case? Case { get; private set; }

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
        string filePath,
        bool isAssigned,
        short courtTypeCode,
        short hijriYear,
        int orderNumber
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
        FilePath = filePath;
        IsAssigned = isAssigned;
        CourtTypeCode = courtTypeCode;
        HijriYear = hijriYear;
        OrderNumber = orderNumber;
    }

    public static Result<Contract> Create(
        Guid caseId,
        CourtType courtType,
        ContractType type,
        DateOnly? issuedOn,
        DateOnly? expiresOn,
        decimal baseAmount,
        decimal initialPayment,
        string filePath,
        bool isAssigned,
        short hijriYear,
        int orderNumber
    )
    {
        if (caseId == Guid.Empty)
            return ContractErrors.InvalidCaseId;

        if (courtType is null)
            return ContractErrors.InvalidCourtType;

        if (!Enum.IsDefined(typeof(ContractType), type))
            return ContractErrors.InvalidCourtType;

        if (string.IsNullOrWhiteSpace(filePath))
            return ContractErrors.MissingFilePath;

        if (baseAmount < 0)
            return ContractErrors.InvalidBaseAmount;

        if (initialPayment < 0)
            return ContractErrors.InvalidInitialPayment;

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
            filePath,
            isAssigned,
            courtType.Code,
            hijriYear,
            orderNumber
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
        return $"ع{hijriYear:D2}-{courtType.Code}-{orderNumber:D4}";
    }

    public override string ToString() => $"Contract #{ContractNumber}";
}
