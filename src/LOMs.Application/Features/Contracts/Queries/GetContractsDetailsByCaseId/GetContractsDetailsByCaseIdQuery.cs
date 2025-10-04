using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Contracts.Queries;

public sealed record GetContractsDetailsByCaseIdQuery(Guid CaseId)
    : IQuery<Result<List<ContractDetailsDto>>>;

