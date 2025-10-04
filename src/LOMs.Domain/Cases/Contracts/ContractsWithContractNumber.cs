    namespace LOMs.Domain.Cases.Contracts;

    public sealed class ContractsWithContractNumber
    {
        public Guid Id { get; set; }
        public Guid CaseId { get; set; }
        public short HijriYear { get; set; }
        public short CourtTypeCode { get; set; }
        public int Type { get; set; } // ContractType enum underlying value
        public DateOnly? IssuedOn { get; set; }
        public DateOnly? ExpiresOn { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal InitialPayment { get; set; }
        public decimal TaxAmount { get; set; }
        public bool IsAssigned { get; set; }

        // Audit fields
        public DateTime CreatedAtUtc { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? LastModifiedUtc { get; set; }
        public string? LastModifiedBy { get; set; } = string.Empty;

        // View-specific fields
        public int RowNumber { get; set; }
        public string ContractNumber { get; set; } = null!;
    }
