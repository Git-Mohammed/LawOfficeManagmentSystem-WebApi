using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Judgments.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Judgments.Queries;

public sealed record GetJudgmentByIdQuery(Guid CaseId, Guid JudgmentId) : IQuery<Result<JudgmentDto>>;