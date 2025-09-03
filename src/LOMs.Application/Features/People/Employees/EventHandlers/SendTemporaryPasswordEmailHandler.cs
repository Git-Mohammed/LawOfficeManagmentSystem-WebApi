using LiteBus.Events.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Domain.People.Employees.DomainEvents;

namespace LOMs.Application.Features.People.Employees.EventHandlers;

public class SendTemporaryPasswordEmailHandler(IEmailSender emailSender) : IEventHandler<EmployeeCreatedEvent>
{
    private readonly IEmailSender _emailSender = emailSender;

    public async Task HandleAsync(EmployeeCreatedEvent @event, CancellationToken cancellationToken)
    {
        var subject = "Your temporary password";
        var body = $"Hello! Your temporary password is {@event.TemporaryPassword}";
        await _emailSender.SendEmailAsync(@event.Email, subject, body);
    }
}