using LiteBus.Commands.Abstractions;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.CourtSessions.Commands;

public sealed record CreateCourtSessionCommand(
    Guid CaseId,
    Guid AssignedEmployeeId,
    DateTimeOffset ScheduledAt,
    string AssignedTasks
) : ICommand<Result<Guid>>;
