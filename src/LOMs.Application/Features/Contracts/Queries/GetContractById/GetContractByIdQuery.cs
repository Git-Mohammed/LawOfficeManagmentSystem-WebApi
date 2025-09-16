using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Contracts.Queries.GetContractById;

public sealed record GetContractByIdQuery(Guid CaseId,
    Guid ContractId
) : IQuery<Result<ContractDto>>;
