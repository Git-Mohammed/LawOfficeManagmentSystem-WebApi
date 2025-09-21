using LiteBus.Commands.Abstractions;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.POAs.Commands.CreatePOA;

public record CreatePOACommand(Guid CaseId, string POANumber, DateOnly IssueDate, string IssuingAuthority,
    CreatePOAFileWithPOACommand POAFile)
    : ICommand<Result<Guid>>;
