
using LiteBus.Commands.Abstractions;

namespace LOMs.Application.Features.Contracts.Commands.CreateContract;
public sealed record CreateContractFileWithContractCommand(
    string FileName,
    Stream File,
    string ContentType
) : ICommand<Guid>;
