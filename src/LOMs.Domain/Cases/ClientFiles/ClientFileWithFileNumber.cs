namespace LOMs.Domain.Cases.ClientFiles
{
    public sealed class ClientFileWithFileNumber
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public short HijriYear { get; set; }
        public short CourtTypeCode { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? LastModifiedUtc { get; set; }
        public string? LastModifiedBy { get; set; }
        public int RowNumber { get; set; }
        public string FileNumber { get; set; } = string.Empty;
    }

}
