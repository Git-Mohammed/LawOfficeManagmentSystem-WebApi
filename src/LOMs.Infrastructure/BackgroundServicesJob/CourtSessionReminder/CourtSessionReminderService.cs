using LOMs.Application.Common.Interfaces;
using LOMs.Infrastructure.BackgroundServicesJob.CourtSessionReminder;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LOMs.Infrastructure.BackgroundJobs;

public class CourtSessionReminderService(
    IServiceScopeFactory scopeFactory,
    ILogger<CourtSessionReminderService> logger,
    TimeProvider timeProvider) : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly ILogger<CourtSessionReminderService> _logger = logger;
    private readonly TimeProvider _time = timeProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Sending initial court session reminders at {Now}", _time.GetUtcNow());

        await SendRemindersAsync(stoppingToken); // Run immediately on startup

        using var timer = new PeriodicTimer(TimeSpan.FromDays(1));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            _logger.LogInformation("Sending daily court session reminders at {Now}", _time.GetUtcNow());
            await SendRemindersAsync(stoppingToken);
        }
    }

    private async Task SendRemindersAsync(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
            var email = scope.ServiceProvider.GetRequiredService<IEmailSender>();

            var tomorrow = _time.GetUtcNow().Date.AddDays(1);

            var sessions = await db.CourtSessions
                .Include(cs => cs.AssignedEmployee)
                    .ThenInclude(e => e!.Person!)
                .Where(cs => cs.ScheduledAt.Date == tomorrow)
                .Select(cs => new CourtSessionReminderDto
                {
                    SessionId = cs.Id,
                    ScheduledAt = cs.ScheduledAt,
                    AssignedTasks = cs.AssignedTasks,
                    AssignedEmployeeId = cs.AssignedEmployee != null ? cs.AssignedEmployee.Id : Guid.Empty,
                    AssignedEmployeeEmail = cs.AssignedEmployee!.Email ?? string.Empty,
                    AssignedEmployeeFullName = cs.AssignedEmployee.Person.FullName ?? string.Empty
                })
                .ToListAsync(stoppingToken);

            if (sessions.Count == 0)
            {
                _logger.LogInformation("No court sessions scheduled for tomorrow.");
                return;
            }

            foreach (var session in sessions)
            {
                if (string.IsNullOrWhiteSpace(session.AssignedEmployeeEmail) || string.IsNullOrWhiteSpace(session.AssignedEmployeeFullName))
                {
                    _logger.LogWarning("Skipping reminder: Missing email or name for session {SessionId}", session.SessionId);
                    continue;
                }

                var subject = $"Reminder: Court Session Scheduled for {session.ScheduledAt:dddd, MMM dd}";
                var body = BuildEmailBody(session);

                try
                {
                    await email.SendEmailAsync(session.AssignedEmployeeEmail, subject, body);
                    _logger.LogInformation("Reminder sent to {Email} for session {SessionId}", session.AssignedEmployeeEmail, session.SessionId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send reminder for session {SessionId}", session.SessionId);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending court session reminders.");
        }
    }

    private static string BuildEmailBody(CourtSessionReminderDto session) => $"""
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
</head>
<body style="font-family: 'Segoe UI', sans-serif; background-color: #f9f9f9; color: #333; margin: 0; padding: 0;">
    <div style="max-width: 600px; margin: auto; background-color: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0,0,0,0.05);">
        
        <!-- English Section -->
        <div style="text-align: left;">
            <h2 style="color: #0056b3;">Court Session Reminder</h2>
            <p>Dear <strong>{session.AssignedEmployeeFullName}</strong>,</p>
            <p>You have a court session scheduled on <strong>{session.ScheduledAt:dddd, MMM dd yyyy at hh:mm tt}</strong>.</p>
            <div style="background-color: #f1f1f1; padding: 10px; border-radius: 6px;">
                <p><strong>Assigned Tasks:</strong></p>
                <p>{session.AssignedTasks}</p>
            </div>
            <p>Please ensure you're prepared and arrive on time.</p>
        </div>

        <hr style="margin: 30px 0;">

        <!-- Arabic Section -->
        <div style="direction: rtl; text-align: right;">
            <h2 style="color: #0056b3;">تذكير بجلسة المحكمة</h2>
            <p>عزيزي <strong>{session.AssignedEmployeeFullName}</strong>،</p>
            <p>لديك جلسة محكمة مجدولة في <strong>{session.ScheduledAt:dddd، dd MMM yyyy الساعة hh:mm tt}</strong>.</p>
            <div style="background-color: #f1f1f1; padding: 10px; border-radius: 6px;">
                <p><strong>المهام الموكلة:</strong></p>
                <p>{session.AssignedTasks}</p>
            </div>
            <p>يرجى التأكد من الاستعداد والحضور في الوقت المحدد.</p>
        </div>

        <!-- Footer -->
        <div style="text-align: center; font-size: 12px; color: #777; margin-top: 30px;">
           ArammTech &copy; {DateTime.UtcNow.Year}
        </div>
    </div>
</body>
</html>
""";
}
