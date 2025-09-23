using LOMs.Domain.Common;
using LOMs.Domain.Common.Extensions;
using LOMs.Domain.Common.Results;
using LOMs.Domain.People.Clients;
using LOMs.Domain.People.Employees;
using LOMs.Domain.People.Phones;
using System.Text.RegularExpressions;

namespace LOMs.Domain.People;

public sealed class Person : AuditableEntity
{
    public string FullName { get; private set; } = null!;
    public string NationalId { get; private set; } = null!;
    public DateOnly BirthDate { get; private set; }
    public string? Address { get; private set; }
    public CountryCode CountryCode { get; private set; }

    private readonly List<Phone> _phones = [];
    public IEnumerable<Phone> Phones => _phones.AsReadOnly();
    public Client? Client { get; private set; }
    public Employee? Employee { get; private set; }

    private Person() { }

    private Person(Guid id, string fullName, string nationalId, CountryCode countryCode, DateOnly birthDate,string address, List<Phone> phones)
        : base(id)
    {
        FullName = fullName;
        NationalId = nationalId;
        BirthDate = birthDate;
        Address = address;
        CountryCode = countryCode;
        _phones = phones;
    }

    public static Result<Person> Create(
        Guid id,
        string fullName,
        string nationalId,
        string countryCode,
        DateOnly birthDate,
        string address,
        List<Phone> phones)
    {
        if (string.IsNullOrWhiteSpace(fullName) || fullName.Length < 3)
            return PersonErrors.FullNameRequired;

        if (string.IsNullOrWhiteSpace(nationalId) || !Regex.IsMatch(nationalId, @"^\d{10}$"))
            return PersonErrors.InvalidNationalId;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        if (birthDate > today)
            return PersonErrors.InvalidBirthDate;

        var age = today.Year - birthDate.Year;

        // Adjust if birthday hasn't occurred yet this year
        if (birthDate > today.AddYears(-age))
            age--;

        if (age < 1)
            return PersonErrors.InvalidBirthDate;

        if (string.IsNullOrWhiteSpace(countryCode))
            return PersonErrors.CountryCodeRequired;

        countryCode = countryCode.Trim().ToUpper();
        CountryCode code;
        try
        {
            code = countryCode.ToEnum<CountryCode>();
        }
        catch
        {
            return PersonErrors.CountryCodeInvalid;
        }

        if(phones == null || phones.Count == 0)
            return PersonErrors.PhoneNumberRequired;

        var person = new Person(id, fullName, nationalId, code, birthDate, address ,phones);

        return person;
    }


    public Result<Updated> Update(string fullName, string nationalId, DateOnly birthDate, string address)
    {
        if (string.IsNullOrWhiteSpace(fullName) || fullName.Length < 3)
            return PersonErrors.FullNameRequired;

        if (string.IsNullOrWhiteSpace(nationalId) || !Regex.IsMatch(nationalId, @"^\d{10}$"))
            return PersonErrors.InvalidNationalId;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - birthDate.Year;
        if (birthDate > today || age < 18)
            return PersonErrors.InvalidBirthDate;

        if (string.IsNullOrWhiteSpace(address) || address.Length < 10)
            return PersonErrors.InvalidAddress;

        FullName = fullName;
        NationalId = nationalId;
        BirthDate = birthDate;
        Address = address;

        return Result.Updated;
    }

}
