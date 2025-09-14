using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Auth.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Auth.Queries.GetUserInfo;

public sealed record GetUserByIdQuery(string? UserId) : IQuery<Result<AppUserDto>>;