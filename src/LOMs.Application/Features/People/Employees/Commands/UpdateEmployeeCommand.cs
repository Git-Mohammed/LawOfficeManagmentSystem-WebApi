using LiteBus.Commands.Abstractions;
using LOMs.Domain.Common.Results;
using LOMs.Domain.Identity;

namespace LOMs.Application.Features.People.Employees.Commands;

public sealed record UpdateEmployeeCommand(
    Guid Id,
    string FullName,
    string NationalId,
    DateOnly BirthDate,
    string  PhoneNumber,
    string PhoneCode,
    string Address,
    string CountryCode,
    Role Role,
    string Email,
    bool IsActive
):ICommand<Result<bool>>;