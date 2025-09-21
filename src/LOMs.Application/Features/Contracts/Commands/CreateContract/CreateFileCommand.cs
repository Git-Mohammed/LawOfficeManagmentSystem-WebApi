
using LiteBus.Commands.Abstractions;

namespace LOMs.Application.Features.Contracts.Commands.CreateContract;
public sealed record CreateFileCommand(
    string FileName,
    Stream File,
    string ContentType
) : ICommand<Guid>;
