using LiteBus.Commands.Abstractions;
using LOMs.Application.Features.Cases.Dtos;
using LOMs.Domain.Common.Results;
using System.Windows.Input;

namespace LOMs.Application.Features.Cases.Commands.CreateCase;

    public sealed record CreateContractWithCaseCommand(
        int ContractType,
        DateOnly? IssueDate,
        DateOnly? ExpiryDate,
        decimal TotalAmount,
        decimal InitialPayment,
        string ContractFilePath,
        bool IsAssigned
    ) : ICommand<Result<ContractDto>>;

