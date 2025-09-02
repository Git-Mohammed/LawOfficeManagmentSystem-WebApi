using LOMs.Domain.Common;
using LOMs.Domain.Common.Extensions;
using LOMs.Domain.Common.Results;
using LOMs.Domain.Identity;

namespace LOMs.Domain.People.Employees;

public sealed class Employee : AuditableEntity
{
    public Guid Id { get;}
    public Guid PersonId { get; }
    public Person Person { get;} = null!;
    public Role Role { get;}

    public string UserId { get; private set; }
    
    private Employee(){}

    private Employee(Guid id, Person person, Role role) : base(id)
    {
        Id = id;
        Person = person ?? throw new ArgumentNullException(nameof(person));
        PersonId = person.Id;
        Role = role;
    }

    public static Result<Employee> Create(Guid id, Person person, string role)
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
        
        return new Employee(id, person, roleEnum);
    }

    public Result<bool> AssignUser(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return EmployeeErrors.IdRequired;
        UserId = id;
        return true;
    }
    
    
}

