using LiteBus.Commands.Abstractions;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.People.Commands;

/// <summary>
/// Command to create a phone record for a person.
/// </summary>
public sealed record CreatePhoneCommand(
    string Number,
    string Code
) : ICommand<Result<Guid>>;
