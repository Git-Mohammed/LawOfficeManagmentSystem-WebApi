using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Cases.Enums.CourtTypes;
using LOMs.Domain.Common;
using LOMs.Domain.Common.Constants;
using LOMs.Domain.Common.Results;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace LOMs.Domain.Cases.Contracts
{
    public sealed class Contract : AuditableEntity
    {
        public Guid CaseId { get; private set; }
        public string ContractNumber { get; private set; } = null!;

        /// <summary>
        /// Display version of the contract number with Arabic prefix.
        /// </summary>
        [NotMapped]
        public string DisplayContractNumber => ConvertToArabicPrefix(ContractNumber);

        public ContractType Type { get; private set; }
        public DateOnly? IssuedOn { get; private set; }
        public DateOnly? ExpiresOn { get; private set; }

        public decimal TotalAmount { get; private set; }
        public decimal InitialPayment { get; private set; }
        public decimal TaxAmount { get; private set; }

        public string FilePath { get; private set; } = null!;
        public bool IsAssigned { get; private set; }

        public Case? Case { get;  set; }

        private Contract() { }

        private Contract(
            Guid id,
            Guid caseId,
            string contractNumber,
            ContractType type,
            DateOnly? issuedOn,
            DateOnly? expiresOn,
            decimal totalAmount,
            decimal initialPayment,
            string filePath,
            bool isAssigned
        ) : base(id)
        {
            CaseId = caseId;
            ContractNumber = contractNumber;
            Type = type;
            IssuedOn = issuedOn;
            ExpiresOn = expiresOn;
            TotalAmount = totalAmount;
            InitialPayment = initialPayment;
            TaxAmount = CalculateTax(totalAmount);
            FilePath = filePath;
            IsAssigned = isAssigned;
        }

        /// <summary>
        /// Factory method to create a new Contract with validation and auto-generated number.
        /// </summary>
        public static Result<Contract> Create(
            Guid id,
            Guid caseId,
            CourtType courtType,
            ContractType type,
            DateOnly? issuedOn,
            DateOnly? expiresOn,
            decimal totalAmount,
            decimal initialPayment,
            string filePath,
            bool isAssigned
        )
        {
            if (id == Guid.Empty)
                return ContractErrors.Invalid_Id;

            if (caseId == Guid.Empty)
                return ContractErrors.Invalid_CaseId;

            if (!Enum.IsDefined(typeof(ContractType), type))
                return ContractErrors.Invalid_ContractType;

            if (!Enum.IsDefined(typeof(CourtType), courtType))
                return ContractErrors.Invalid_CourtType;

            if (string.IsNullOrWhiteSpace(filePath))
                return ContractErrors.Missing_FilePath;

            if (totalAmount < 0)
                return ContractErrors.Invalid_TotalAmount;

            if (initialPayment < 0)
                return ContractErrors.Invalid_InitialPayment;

            var contractNumber = GenerateContractNumber(id, courtType);

            var contract = new Contract(
                id,
                caseId,
                contractNumber,
                type,
                issuedOn,
                expiresOn,
                totalAmount,
                initialPayment,
                filePath,
                isAssigned
            );

            return contract;
        }

        private static decimal CalculateTax(decimal amount) =>
            Math.Round(amount * LOMsConstants.TaxRate, 2);

        /// <summary>
        /// Generates a contract number in the format: A-YY-CourtTypeId-XXXXXX
        /// Stored with 'A' in DB, displayed as 'ع' in UI.
        /// </summary>
        private static string GenerateContractNumber(Guid id, CourtType courtType)
        {
            var hijriCalendar = new HijriCalendar();
            var hijriYear = hijriCalendar.GetYear(DateTime.UtcNow) % 100;
            var guidSegment = id.ToString("N")[..6];

            // Stored format: A47-200-XXXXXX
            return $"A{hijriYear:D2}-{(int)courtType}-{guidSegment}";
        }


        /// <summary>
        /// Converts stored 'A' prefix to Arabic 'ع' for display.
        /// </summary>
        private static string ConvertToArabicPrefix(string storedNumber)
        {
            if (string.IsNullOrWhiteSpace(storedNumber))
                return storedNumber;

            const char RLE = '\u202B'; // Right-to-Left Embedding
            const char PDF = '\u202C'; // Pop Directional Formatting

            var arabicFormatted = storedNumber.StartsWith("A")
                ? "ع" + storedNumber.Substring(1)
                : storedNumber;

            return $"{RLE}{arabicFormatted}{PDF}";
        }

        public override string ToString() => $"Contract #{DisplayContractNumber}";
    }
}
