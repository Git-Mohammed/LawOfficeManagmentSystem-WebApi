using LiteBus.Commands.Abstractions;
using LOMs.Application.Features.Cases.Dtos;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Cases.Commands.CreateCase;

public sealed record CreateContractWithCaseCommand(
    ContractType ContractType,
    DateOnly? IssueDate,
    DateOnly? ExpiryDate,
    decimal BaseAmount,
    decimal InitialPayment,
    Stream AttachmentFileStream,  // Use a generic Stream
    string AttachmentFileName,    // Use a string for the file name
    bool IsAssigned
);