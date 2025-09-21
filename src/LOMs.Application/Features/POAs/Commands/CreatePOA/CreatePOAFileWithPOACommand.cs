
using LiteBus.Commands.Abstractions;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.POAs.Commands.CreatePOA;

public sealed record CreatePOAFileWithPOACommand(
    string FileName,
    Stream File,
    string ContentType
) : ICommand<Result<Guid>>;
