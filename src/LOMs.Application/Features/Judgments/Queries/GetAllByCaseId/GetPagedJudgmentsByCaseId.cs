using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Models;
using LOMs.Domain.Common.Results;
namespace LOMs.Application.Features.Judgments.Queries.GetAllByCaseId;
public sealed record GetPagedJudgmentsByCaseIdQuery(
    Guid CaseId,
    int PageNumber,
    int PageSize
) : IQuery<Result<PaginatedList<JudgmentDetailsDto>>>;
