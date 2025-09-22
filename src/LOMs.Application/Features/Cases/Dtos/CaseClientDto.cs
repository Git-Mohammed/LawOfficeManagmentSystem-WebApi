public sealed class CaseClientDto
{
    public Guid ClientId { get; init; }
    public string FullName { get; init; } = null!;
    public string NationalId { get; init; } = null!;
    public string PhoneNumber { get; init; } = null!;
    public string CountryCode { get; init; } = null!;
    public string? Address { get; init; }
    public DateOnly Birthdate { get; init; }
}