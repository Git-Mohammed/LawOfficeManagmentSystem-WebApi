namespace LOMs.Application.Features.People.Clients.Dtos
{
    public sealed class PersonDto
    {
        public Guid PersonId { get; init; }
        public string FullName { get; init; } = null!;
        public string NationalId { get; init; } = null!;
        public DateOnly BirthDate { get; init; }
        public PhoneDto Phone { get; init; } = null!;
        public string Address { get; init; } = null!;
        public string CountryCode { get; init; } = null!;
    }
}
