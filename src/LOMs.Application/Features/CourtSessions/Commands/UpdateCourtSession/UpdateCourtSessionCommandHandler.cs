using LiteBus.Commands.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Domain.Cases.CourtSessions;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.CourtSessions.Commands.UpdateCourtSession;

public sealed class UpdateCourtSessionCommandHandler(
    ILogger<UpdateCourtSessionCommandHandler> logger,
    IAppDbContext context
) : ICommandHandler<UpdateCourtSessionCommand, Result<Updated>>
{
    private readonly ILogger<UpdateCourtSessionCommandHandler> _logger = logger;
    private readonly IAppDbContext _context = context;

    public async Task<Result<Updated>> HandleAsync(UpdateCourtSessionCommand message, CancellationToken cancellationToken = default)
    {
        var session = await _context.CourtSessions
            .FirstOrDefaultAsync(cs => cs.Id == message.SessionId && cs.CaseId == message.CaseId, cancellationToken);

        if (session is null)
        {
            _logger.LogWarning("CourtSession update failed: Session not found. SessionId: {SessionId}, CaseId: {CaseId}", message.SessionId, message.CaseId);
            return CourtSessionErrors.SessionNotFound;
        }

        var employeeExists = await _context.Employees
            .AnyAsync(e => e.Id == message.AssignedEmployeeId, cancellationToken);

        if (!employeeExists)
        {
            _logger.LogWarning("CourtSession update failed: Assigned employee not found. EmployeeId: {EmployeeId}", message.AssignedEmployeeId);
            return CourtSessionErrors.EmployeeNotFound;
        }

        // Check for scheduling conflict
        var hasConflict = await _context.CourtSessions
            .AnyAsync(cs =>
                cs.AssignedEmployeeId == message.AssignedEmployeeId &&
                cs.ScheduledAt == message.NewDate,
                cancellationToken);

        if (hasConflict)
        {
            _logger.LogWarning("CourtSession creation failed: Employee already assigned to another session at the same time. EmployeeId: {EmployeeId}, ScheduledAt: {ScheduledAt}",
                message.AssignedEmployeeId, message.NewDate);

            return CourtSessionErrors.EmployeeScheduleConflict;
        }


        var updateResult = session.Update(caseId: message.CaseId,
            assignedEmployeeId: message.AssignedEmployeeId,
            newDate: message.NewDate,
            assignedTasks: message.AssignedTasks
        );

        if (updateResult.IsError)
        {
            _logger.LogWarning("CourtSession update failed: {Errors}", updateResult.Errors);
            return updateResult.Errors;
        }

        _context.CourtSessions.Update(session);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("CourtSession updated successfully (ID: {SessionId})", session.Id);
        return Result.Updated;
    }
}
