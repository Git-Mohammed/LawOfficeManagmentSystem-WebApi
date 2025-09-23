using LiteBus.Commands.Abstractions;
using LiteBus.Events.Abstractions;
using LOMs.Domain.Cases;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Cases.Commands.UpdateStatus;

public sealed class UpdateCaseStatusCommandHandler
    : ICommandHandler<UpdateCaseStatusCommand, Result<Updated>>
{
    private readonly IAppDbContext _context;
    private readonly ILogger<UpdateCaseStatusCommandHandler> _logger;

    public UpdateCaseStatusCommandHandler(
        IAppDbContext context,
        ILogger<UpdateCaseStatusCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Updated>> HandleAsync(UpdateCaseStatusCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to update status for CaseId: {CaseId} to {Status}", command.CaseId, command.Status);

        var caseEntity = await _context.Cases
            .FirstOrDefaultAsync(c => c.Id == command.CaseId, cancellationToken);

        if (caseEntity is null)
        {
            _logger.LogWarning("Case not found: {CaseId}", command.CaseId);
            return CaseErrors.CaseNotFound;
        }

        if (caseEntity.Status == command.Status)
        {
            _logger.LogInformation("Case already has status {Status}, no update needed.", command.Status);
            return Result.Updated;
        }

      var updateResult =  caseEntity.UpdateStatus(command.Status);
        if (updateResult.IsError)
        {
            _logger.LogWarning("Failed to update case status for CaseId: {CaseId}. Errors: {Errors}", command.CaseId, updateResult.Errors);
            return updateResult.Errors;
        }

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Case status updated successfully for CaseId: {CaseId}", command.CaseId);
        
        return Result.Updated;
    }
}
