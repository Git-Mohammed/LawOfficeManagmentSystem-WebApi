using LiteBus.Commands.Abstractions;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Contracts.Commands.CreateContract;

public sealed record CreateContractCommand(
    Guid CaseId,
    ContractType ContractType,
    DateOnly? IssuedOn,
    DateOnly? ExpiresOn,
    decimal TotalAmount,
    decimal InitialPayment,
    bool IsAssigned,
    CreateFileCommand ContractFile
) : ICommand<Result<Guid>>;

