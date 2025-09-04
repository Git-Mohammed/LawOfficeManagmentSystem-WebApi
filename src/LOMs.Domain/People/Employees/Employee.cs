using LOMs.Domain.Common;
using LOMs.Domain.Common.Extensions;
using LOMs.Domain.Common.Results;
using LOMs.Domain.Identity;

namespace LOMs.Domain.People.Employees;

public sealed class Employee : AuditableEntity
{
    public Guid PersonId { get; }
    public Person Person { get; } = null!;
    public Role Role { get; }
    public string Email { get; }
    public string UserId { get; private set; }

    private Employee()
    {
    }

    private Employee(Guid id,string email ,Person person, Role role) : base(id)
    {
        Person = person ?? throw new ArgumentNullException(nameof(person));
        PersonId = person.Id;
        Role = role;
        Email = email ?? throw new ArgumentNullException(nameof(email));;
    }

    public static Result<Employee> Create(Guid id,string email, Person person, string role)
    {
        if (id == Guid.Empty)
            return EmployeeErrors.IdRequired;
        if (person is null)
            return EmployeeErrors.PersonRequired;
        Role roleEnum;
        try
        {
            roleEnum = role.Trim().ToEnum<Role>(ignoreCase: true);
        }
        catch (Exception e)
        {
            return EmployeeErrors.RoleInvalid;
        }

        if (!Enum.IsDefined(roleEnum))
            return EmployeeErrors.RoleInvalid;
        if (string.IsNullOrWhiteSpace(email))
            return EmployeeErrors.EmailRequired;

        return new Employee(id,email,person, roleEnum);
    }

    public Result<bool> AssignUser(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return EmployeeErrors.IdRequired;
        UserId = id;
        return true;
    }
}

