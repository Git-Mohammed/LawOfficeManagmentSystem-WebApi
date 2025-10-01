using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Models;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Judgments.Queries.GetAll;
public sealed record GetPagedJudgmentsQuery(int PageNumber = 1, int PageSize = 10)
    : IQuery<Result<PaginatedList<JudgmentDetailsDto>>>;
