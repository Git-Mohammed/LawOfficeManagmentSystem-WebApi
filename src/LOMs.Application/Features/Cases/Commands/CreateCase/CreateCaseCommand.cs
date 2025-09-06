using LiteBus.Commands.Abstractions;
using LOMs.Application.Features.Cases.Dtos;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Cases.Commands.CreateCase;

public sealed record CreateCaseCommand(List<CaseClientModel> Clients,
    string? CaseNumber,
            string? CaseSubject,
            Guid CourtTypeId,
            PartyRole PartyRole,
            string? ClientRequests,
            DateOnly EstimatedReviewDate,
            string? LawyerOpinion,
            bool IsDraft,
            bool HasContracts,
            List<CreateContractWithCaseCommand> Contracts,
            bool HasPOAs,
            List<CreatePOAWithCaseCommand> POAs,
            Guid AssignedOfEmployeeId) : ICommand<Result<CaseDto>>;
