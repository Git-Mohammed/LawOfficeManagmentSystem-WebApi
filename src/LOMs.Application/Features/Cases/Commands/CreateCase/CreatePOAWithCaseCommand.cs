using LiteBus.Commands.Abstractions;
using LOMs.Application.Features.Cases.Dtos;
using LOMs.Domain.Common.Results;

public sealed record CreatePOAWithCaseCommand(
    string POANumber,
    DateOnly IssueDate,
    string IssuingAuthority,
    Stream AttachmentFileStream,
    string AttachmentFileName
) : ICommand<Result<POADto>>;