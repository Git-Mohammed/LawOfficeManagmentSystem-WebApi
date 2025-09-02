using LOMs.Domain.Common;
using LOMs.Domain.Common.Results;
using LOMs.Domain.Identity;

namespace LOMs.Domain.People.Employees;

public sealed class Employee : AuditableEntity
{
    public Guid PersonId { get; }
    public Person Person { get;} = null!;
    public Role Role { get;}
    
    private Employee(){}

    private Employee(Guid id, Person person, Role role) : base(id)
    {
        Person = person ?? throw new ArgumentNullException(nameof(person));
        PersonId = person.Id;
        Role = role;
    }

    public static Result<Employee> Create(Guid id, Person person, Role role)
    {
        if (id == Guid.Empty)
            return EmployeeErrors.IdRequired;
        if (person is null)
            return EmployeeErrors.PersonRequired;
        if (!Enum.IsDefined(role))
            return EmployeeErrors.RoleInvalid;
        
        return new Employee(id, person, role);
    }

}

