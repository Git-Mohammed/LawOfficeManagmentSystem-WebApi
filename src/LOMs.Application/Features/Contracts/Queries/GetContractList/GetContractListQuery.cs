using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Contracts.Queries.GetContractList;
public sealed class GetContractListQuery : IQuery<Result<List<ContractListItemDto>>>;