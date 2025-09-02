using LiteBus.Commands.Abstractions;
using LOMs.Application.Features.Cases.Dtos;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Cases.Enums.CourtTypes;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Cases.Commands.CreateCase;

public sealed record CreateCaseCommand(List<CaseClientModel> Clients,
    string? CaseNumber,
            string CaseNotes,
            CourtType CourtType,
            PartyRole Role,
            string ClientRequests,
            DateOnly? EstimatedReviewDate,
            string? LawyerOpinion,
            bool IsDraft,
            string AssignedOfficer) : ICommand<Result<CaseDto>>;
