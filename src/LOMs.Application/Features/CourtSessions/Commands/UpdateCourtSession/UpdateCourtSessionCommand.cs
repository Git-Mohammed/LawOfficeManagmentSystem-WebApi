using LiteBus.Commands.Abstractions;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.CourtSessions.Commands.UpdateCourtSession;
public sealed record UpdateCourtSessionCommand(
    Guid CaseId,
    Guid SessionId,
    DateTimeOffset NewDate,
    string AssignedTasks,
    Guid AssignedEmployeeId)
    : ICommand<Result<Updated>>;
