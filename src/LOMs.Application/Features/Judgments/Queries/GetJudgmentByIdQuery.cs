using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Judgments.DTOs;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Judgments.Queries;

public sealed record GetJudgmentByIdQuery(Guid CaseId, Guid JudgmentId) : IQuery<Result<JudgmentDTo>>;