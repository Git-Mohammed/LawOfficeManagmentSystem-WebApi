using LiteBus.Commands.Abstractions;
using LOMs.Application.Features.Cases.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Cases.Commands.CreateCase;

public sealed record CreatePOAWithCaseCommand(
    string POANumber,
    DateOnly IssueDate,
    string IssuingAuthority,
    string AttachmentFilePath
) : ICommand<Result<POADto>>;
