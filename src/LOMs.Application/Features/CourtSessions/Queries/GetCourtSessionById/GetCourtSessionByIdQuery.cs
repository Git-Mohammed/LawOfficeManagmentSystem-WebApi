using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.CourtSessions.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.CourtSessions.Queries.GetCourtSessionById;


public sealed record GetCourtSessionByIdQuery(Guid CaseId, Guid SessionId)
    : IQuery<Result<CourtSessionDto>>;

