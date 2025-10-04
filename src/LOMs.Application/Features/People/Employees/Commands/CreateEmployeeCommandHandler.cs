using LiteBus.Commands.Abstractions;
using LOMs.Domain.Identity;
using LOMs.Application.Common.Interfaces;
using LOMs.Domain.Common.Results;
using LOMs.Domain.People;
using LOMs.Domain.People.Employees;
using LOMs.Domain.People.Employees.DomainEvents;
using Microsoft.EntityFrameworkCore;
using LOMs.Application.Features.People.Employees.Dtos;
using LOMs.Domain.People.Phones;

namespace LOMs.Application.Features.People.Employees.Commands;

public class CreateEmployeeCommandHandler(IAppDbContext context, IIdentityService identityService, IMapper mapper, IPasswordGenerator passwordGenerator, IDomainEventPublisher  eventPublisher)
    : ICommandHandler<CreateEmployeeCommand, Result<EmployeeDto>>
{
    private readonly IAppDbContext _context = context;
    private readonly IIdentityService _identityService = identityService;
    private readonly IMapper _mapper = mapper;
    private readonly IPasswordGenerator _passwordGenerator = passwordGenerator;
    private readonly IDomainEventPublisher _eventPublisher = eventPublisher;

    public async Task<Result<EmployeeDto>> HandleAsync(CreateEmployeeCommand command, CancellationToken cancellationToken = new CancellationToken())
    {
        // check uniqueness
        var nationalId = command.Person.NationalId.Trim().ToLower();
        if (await _context.People.AnyAsync(p => p.NationalId == nationalId, cancellationToken))
            return PersonErrors.ExistingNationalId;
        
        var email = command.Email.Trim().ToLower();
        if (await _context.Employees.AnyAsync(e => e.Email == email, cancellationToken))
            return EmployeeErrors.ExistingEmail;



        var phoneNumber = command.Person.Phone.Number?.Trim();
        var code = command.Person.Phone.Code?.Trim();

        if (string.IsNullOrWhiteSpace(phoneNumber))
            return PersonErrors.PhoneNumberRequired;
        if (string.IsNullOrWhiteSpace(code))
            return PersonErrors.PhoneCodeRequired;

        var exists = await _context.Phones.AnyAsync(x => (x.Number == phoneNumber) && (x.Code == code), cancellationToken);
        if (exists)
            return PersonErrors.PhoneNumberAlreadyExists;

        var phoneResult = Phone.Create(Guid.NewGuid(), phoneNumber, code);

        if (phoneResult.IsError)
            return phoneResult.Errors;

        // create person
        var person = Person.Create(
            Guid.NewGuid(),
            command.Person.FullName,
            nationalId,
            command.Person.CountryCode,
            command.Person.BirthDate,
            command.Person.Address,
            new List<Phone> { phoneResult.Value }
        );
        if (person.IsError)
            return person.Errors;
        
        // create employee
        var employee = Employee.Create(Guid.NewGuid(),email,person.Value, command.Role);
        
        if (employee.IsError)
            return employee.Errors;
        
        // create userf
        var randomPassword = _passwordGenerator.GenerateTempPassword();
        var userResult = await _identityService.CreateUserAsync(nationalId, email, randomPassword, employee.Value.Role.ToString());

        if (userResult.IsError || string.IsNullOrEmpty(userResult.Value))
        {
            return userResult.Errors;
        }

        var assignResult = employee.Value.AssignUser(userResult.Value);
        if (assignResult.IsError)
        {
            return assignResult.Errors;
        }
        
        _context.Employees.Add(employee.Value);
        await _context.SaveChangesAsync(cancellationToken);
        
        // send email
        var createdEmployee = employee.Value;
        await _eventPublisher.PublishAsync(new EmployeeCreatedEvent(createdEmployee, createdEmployee!.Email!, randomPassword), cancellationToken);
        
        return _mapper.Map<Employee,EmployeeDto>(createdEmployee);
        
    }
}