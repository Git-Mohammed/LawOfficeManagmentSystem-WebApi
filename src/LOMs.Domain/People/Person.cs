using System.Text.RegularExpressions;
using LOMs.Domain.Common;
using LOMs.Domain.Common.Results;
using LOMs.Domain.People.Clients;

namespace LOMs.Domain.People;

public sealed class Person : AuditableEntity
{
    public string FullName { get; private set; } = null!;
    public string NationalId { get; private set; } = null!;
    public DateOnly BirthDate { get; private set; }
    public string PhoneNumber { get; private set; } = null!;
    public string? Address { get; private set; }

    public Client? Client { get; set; }
    private Person() { }

    private Person(Guid id, string fullName, string nationalId, DateOnly birthDate, string phoneNumber, string address)
        : base(id)
    {
        FullName = fullName;
        NationalId = nationalId;
        BirthDate = birthDate;
        PhoneNumber = phoneNumber;
        Address = address;
    }

    public static Result<Person> Create(Guid id, string fullName, string nationalId, DateOnly birthDate, string phoneNumber, string address)
    {
        if (string.IsNullOrWhiteSpace(fullName) || fullName.Length < 3)
            return PersonErrors.FullNameRequired;

        if (string.IsNullOrWhiteSpace(nationalId) || !Regex.IsMatch(nationalId, @"^\d{10}$"))
            return PersonErrors.InvalidNationalId;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - birthDate.Year;
        if (birthDate > today || age < 18)
            return PersonErrors.InvalidBirthDate;

        if (phoneNumber.StartsWith("05") && (string.IsNullOrWhiteSpace(phoneNumber) || !Regex.IsMatch(phoneNumber, @"^\+?\d{7,15}$")))
            return PersonErrors.InvalidPhoneNumber;


        return new Person(id, fullName, nationalId, birthDate, phoneNumber, address);
    }

    public Result<Updated> Update(string fullName, string nationalId, DateOnly birthDate, string phoneNumber, string address)
    {
        if (string.IsNullOrWhiteSpace(fullName) || fullName.Length < 3)
            return PersonErrors.FullNameRequired;

        if (string.IsNullOrWhiteSpace(nationalId) || !Regex.IsMatch(nationalId, @"^\d{11}$"))
            return PersonErrors.InvalidNationalId;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - birthDate.Year;
        if (birthDate > today || age < 18)
            return PersonErrors.InvalidBirthDate;

        if (string.IsNullOrWhiteSpace(phoneNumber) || !Regex.IsMatch(phoneNumber, @"^\+?\d{7,15}$"))
            return PersonErrors.InvalidPhoneNumber;

        if (string.IsNullOrWhiteSpace(address) || address.Length < 10)
            return PersonErrors.InvalidAddress;

        FullName = fullName;
        NationalId = nationalId;
        BirthDate = birthDate;
        PhoneNumber = phoneNumber;
        Address = address;

        return Result.Updated;
    }
}
