public sealed class CaseClientDto
{
    public Guid ClientId { get; init; }
    public string FullName { get; init; } = null!;
    public string NationalId { get; init; } = null!;
    public PhoneDto PhoneNumber { get; init; } = null!;
    public string CountryCode { get; init; } = null!;
    public string? Address { get; init; }
    public DateOnly Birthdate { get; init; }
}

public sealed class PhoneDto
{
    public Guid PhoneId { get; init; }
    public string Number { get; init; } = null!;
    public string Code { get; init; } = null!;
}