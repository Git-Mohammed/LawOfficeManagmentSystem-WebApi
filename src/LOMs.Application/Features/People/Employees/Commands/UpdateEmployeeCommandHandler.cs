using LiteBus.Commands.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Domain.Common.Results;
using LOMs.Domain.People;
using LOMs.Domain.People.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.People.Employees.Commands;

public class UpdateEmployeeCommandHandler(IAppDbContext context, ILogger<UpdateEmployeeCommandHandler>  logger, IIdentityService identityService): ICommandHandler<UpdateEmployeeCommand, Result<bool>>
{
    private readonly IAppDbContext _context = context;
    private readonly ILogger<UpdateEmployeeCommandHandler> _logger = logger;
    private readonly IIdentityService _identityService = identityService;

    public async Task<Result<bool>> HandleAsync(UpdateEmployeeCommand command, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        { 
            var existingEmployee = await _context.Employees.Include(e=> e.Person).ThenInclude(p=> p.Phones).FirstOrDefaultAsync(e=> e.Id == command.Id, cancellationToken: cancellationToken);
            if (existingEmployee == null) return Error.NotFound();
            
            if(existingEmployee.Email != command.Email)
            {
                var isExistEmployeeHaveSameEmail = await _context.Employees.AnyAsync(e => e.Email == command.Email && e.Id != existingEmployee.Id, cancellationToken);
                if(isExistEmployeeHaveSameEmail) return Error.Conflict("EmailUsedAntherExistsEmployee","Email already exists");
            }

            if (existingEmployee.Person.NationalId != command.NationalId)
            {
                var isExistEmployeeHaveSameNationalId = await _context.Employees.Include(e=> e.Person).AnyAsync(e=> e.Person.NationalId == command.NationalId && e.Id != existingEmployee.Id, cancellationToken);
                if(isExistEmployeeHaveSameNationalId) return Error.Conflict("NationalIdUsedAnotherExistsEmployee","National id already exists");
            }
            
            var updatePersonResult =  existingEmployee.Person.Update(command.FullName, command.NationalId, command.BirthDate, command.Address);
            if(updatePersonResult.IsError) return updatePersonResult.Errors;
           
            var phone = existingEmployee.Person.Phones.FirstOrDefault();
            if (phone is not null)
            {
                var updatePhoneResult =  phone.Update(command.PhoneNumber, command.CountryCode);
                if(updatePhoneResult.IsError) return updatePhoneResult.Errors;
            }

            var updateEmployeeResult = existingEmployee.Update(command.Email, command.Role);
            if(updateEmployeeResult.IsError) return updateEmployeeResult.Errors;

            var user = await _identityService.GetUserByIdAsync(existingEmployee.UserId);
            if(user.IsError || user.Value is null) return user.Errors;
            var updateUserResult = await _identityService.UpdateUserAsync(user.Value.UserId, user.Value.Username, user.Value.Email, existingEmployee.Role.ToString());
            if(updateUserResult.IsError) return updateUserResult.Errors;

            var wasActive = existingEmployee.IsActive; // الحالة القديمة

            if (command.IsActive != wasActive)
            {
                existingEmployee.ActivateOrDeactivate(command.IsActive);

                if (command.IsActive && !wasActive)
                { 
                    // الموظف كان غير نشط وبقى نشط → Unblock
                    var result = await _identityService.UnblockUserAsync(user.Value.UserId);
                    if (result.IsError) return result.Errors;
                }
                else if (!command.IsActive && wasActive)
                {
                    // الموظف كان نشط وبقى غير نشط → Block
                    var result = await _identityService.BlockUserAsync(user.Value.UserId);
                    if (result.IsError) return result.Errors;
                }
            }
            
            _context.Employees.Update(existingEmployee);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}