using LOMs.Domain.Cases;
using LOMs.Domain.Common;
using LOMs.Domain.Common.Extensions;
using LOMs.Domain.Common.Results;
using LOMs.Domain.Identity;

namespace LOMs.Domain.People.Employees;

public sealed class Employee : AuditableEntity
{
    public Guid PersonId { get; }
    public Person? Person { get; set; } = null!;
    public Role Role { get; private set; }
    public string? Email { get; private set; }
    public string? UserId { get; private set; }
    public bool IsActive { get; private set; }

    public ICollection<Case> Cases { get; set; } = [];
    private Employee()
    {
    }

    private Employee(Guid id,string email ,Person person, Role role,bool isActive) : base(id)
    {
        Person = person ?? throw new ArgumentNullException(nameof(person));
        PersonId = person.Id;
        Role = role;
        Email = email ?? throw new ArgumentNullException(nameof(email));;
        IsActive = isActive;
    }

    public static Result<Employee> Create(Guid id,string email, Person person, Role role)
    {
        if (id == Guid.Empty)
            return EmployeeErrors.IdRequired;
        if (person is null)
            return EmployeeErrors.PersonRequired;
      
        if (!Enum.IsDefined(role))
            return EmployeeErrors.RoleInvalid;
        if (string.IsNullOrWhiteSpace(email))
            return EmployeeErrors.EmailRequired;

        return new Employee(id,email,person, role,true);
    }

    public Result<bool> AssignUser(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return EmployeeErrors.IdRequired;
        UserId = id;
        return true;
    }

    public Result<Updated> Update(string email, string role)
    {
        Role roleEnum;
        try
        {
            roleEnum = role.Trim().ToEnum<Role>(ignoreCase: true);
        }
        catch (Exception)
        {
            return EmployeeErrors.RoleInvalid;
        }

        if (!Enum.IsDefined(roleEnum))
            return EmployeeErrors.RoleInvalid;
        if (string.IsNullOrWhiteSpace(email))
            return EmployeeErrors.EmailRequired;
        Email = email;
        Role = roleEnum;
        
        return Result.Updated;
    }

    public void ActivateOrDeactivate(bool isActive)
    {
        IsActive = isActive;
    }
    
}

