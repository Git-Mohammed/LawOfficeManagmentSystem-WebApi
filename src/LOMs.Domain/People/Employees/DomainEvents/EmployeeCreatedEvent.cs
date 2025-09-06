namespace LOMs.Domain.People.Employees.DomainEvents;


public record EmployeeCreatedEvent(Employee employee,string email,string tempPassword)
{
    public Employee Employee { get;} = employee;
    public string Email { get; } =  email;
    public string TemporaryPassword { get; } = tempPassword;
}