using LiteBus.Commands.Abstractions;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Cases.Commands.UpdateStatus;
public sealed record UpdateCaseStatusCommand(Guid CaseId, CaseStatus Status)
    : ICommand<Result<Updated>>;
