using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.POAs.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.POAs.Queries.GetById;

public sealed record GetPOAByIdQuery(Guid CaseId, Guid POAId) : IQuery<Result<POADto>>;