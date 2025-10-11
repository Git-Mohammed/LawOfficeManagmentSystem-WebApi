using LiteBus.Commands.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Domain.Cases;
using LOMs.Domain.Cases.CourtSessions;
using LOMs.Domain.Common.Results;
using LOMs.Domain.People.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.CourtSessions.Commands;

public sealed class CreateCourtSessionCommandHandler(
    ILogger<CreateCourtSessionCommandHandler> logger,
    IAppDbContext context
) : ICommandHandler<CreateCourtSessionCommand, Result<Guid>>
{
    private readonly ILogger<CreateCourtSessionCommandHandler> _logger = logger;
    private readonly IAppDbContext _context = context;

    public async Task<Result<Guid>> HandleAsync(CreateCourtSessionCommand message, CancellationToken cancellationToken = default)
    {
        var employeeExists = await _context.Employees
            .AnyAsync(e => e.Id == message.AssignedEmployeeId, cancellationToken);

        if (!employeeExists)
        {
            _logger.LogWarning("CourtSession creation failed: Assigned employee not found. EmployeeId: {EmployeeId}", message.AssignedEmployeeId);
            return CourtSessionErrors.EmployeeNotFound;
        }

        var caseExists = await _context.Cases
            .AnyAsync(c => c.Id == message.CaseId, cancellationToken);

        if (!caseExists)
        {
            _logger.LogWarning("CourtSession creation failed: Case not found. CaseId: {CaseId}", message.CaseId);
            return CourtSessionErrors.CaseNotFound;
        }

        //// Check for scheduling conflict
        //var hasConflict = await _context.CourtSessions
        //    .AnyAsync(cs =>
        //        cs.AssignedEmployeeId == message.AssignedEmployeeId &&
        //        cs.ScheduledAt == message.ScheduledAt,
        //        cancellationToken);

        //if (hasConflict)
        //{
        //    _logger.LogWarning("CourtSession creation failed: Employee already assigned to another session at the same time. EmployeeId: {EmployeeId}, ScheduledAt: {ScheduledAt}",
        //        message.AssignedEmployeeId, message.ScheduledAt);

        //    return CourtSessionErrors.EmployeeScheduleConflict;
        //}

        var sessionResult = CourtSession.Create(
            Guid.NewGuid(),
            caseId: message.CaseId,
            assignedEmployeeId: message.AssignedEmployeeId,
            scheduledAt: message.ScheduledAt,
            assignedTasks: message.AssignedTasks
        );

        if (sessionResult.IsError)
        {
            _logger.LogWarning("CourtSession creation failed: {Errors}", sessionResult.Errors);
            return sessionResult.Errors;
        }

        _context.CourtSessions.Add(sessionResult.Value);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("CourtSession created successfully (ID: {SessionId})", sessionResult.Value.Id);
        return sessionResult.Value.Id;
    }
}
