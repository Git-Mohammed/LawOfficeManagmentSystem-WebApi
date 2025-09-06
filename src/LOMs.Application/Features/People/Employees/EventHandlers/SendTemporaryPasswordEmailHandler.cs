using LiteBus.Events.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Domain.People.Employees.DomainEvents;

namespace LOMs.Application.Features.People.Employees.EventHandlers;

public class SendTemporaryPasswordEmailHandler(IEmailSender emailSender) : IEventHandler<EmployeeCreatedEvent>
{
    private readonly IEmailSender _emailSender = emailSender;

    public async Task HandleAsync(EmployeeCreatedEvent @event, CancellationToken cancellationToken)
    {
        var subject = "Welcome to the our team";
        var body = GetTemplate(@event,subject);
        await _emailSender.SendEmailAsync(@event.Email, subject, body);
    }


    private string GetTemplate(EmployeeCreatedEvent @event, string subject)
    {
        return $@"
            <!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>{subject}</title>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        margin: 0;
                        padding: 0;
                    }}
                    .container {{
                        max-width: 600px;
                        margin: 40px auto;
                        background-color: #ffffff;
                        padding: 30px;
                        border-radius: 8px;
                        box-shadow: 0 0 10px rgba(0,0,0,0.1);
                    }}
                    h2 {{
                        color: #333333;
                    }}
                    p {{
                        font-size: 16px;
                        line-height: 1.5;
                        color: #555555;
                    }}
                    .password {{
                        font-weight: bold;
                        font-size: 18px;
                        color: #000000;
                        background-color: #eaeaea;
                        padding: 5px 10px;
                        border-radius: 4px;
                        display: inline-block;
                    }}
                    .footer {{
                        margin-top: 30px;
                        font-size: 12px;
                        color: #999999;
                    }}
                </style>
            </head>
            <body>
                <div class=""container"">
                    <h2>Hello {@event.Employee.Person.FullName},</h2>
                    <p>Your temporary password is:</p>
                    <p class=""password"">{@event.TemporaryPassword}</p>
                    <p>Please change it after logging in for security reasons.</p>
                    <div class=""footer"">
                        &copy; 2025 MyCompany. All rights reserved.
                    </div>
                </div>
            </body>
            </html>";
    }
}