using LiteBus.Commands.Abstractions;
using LOMs.Application.Features.Contracts.Commands.CreateContract;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Judgments.Commands;

public sealed record CreateJudgmentCommand(
    Guid CaseId,
    string Number,
    string Type,
    string SubType,
    DateOnly IssuedOn,
    DateOnly ExpiresOn,
    CreateFileCommand FileCommand
    ):ICommand<Result<Guid>>;