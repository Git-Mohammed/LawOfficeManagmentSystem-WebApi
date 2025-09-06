using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.People.Clients.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.People.Clients.Queries.GetClientByNationalIdQuery;
public sealed record GetClientByNationalIdQuery(string NationalId)
    : IQuery<Result<ClientDto>>;
