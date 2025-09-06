using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Cases.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Cases.Queries.GetCaseByIdQuery;
public sealed record GetCaseByIdQuery(Guid CaseId)
    : IQuery<Result<CaseDetailsDto>>;
