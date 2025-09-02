namespace LOMs.Domain.People.Employees;

using LOMs.Domain.Common.Results;

public static class EmployeeErrors
{
    public static readonly Error IdRequired =
        Error.Validation("Employee.IdRequired", "Employee Id is required.");

    public static readonly Error PersonRequired =
        Error.Validation("Employee.PersonRequired", "Person is required to create an employee.");

    public static readonly Error RoleInvalid =
        Error.Validation("Employee.RoleInvalid", "The provided role is invalid.");
}