using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.CourtSessions.Dtos;
using LOMs.Domain.Cases.CourtSessions;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.CourtSessions.Queries.GetFutureCourtSessions;

public sealed class GetFutureCourtSessionsQuery : IQuery<Result<IReadOnlyList<GetFutureCourtSessionDto>>>
{
    public DateTimeOffset ReferenceTime { get; }

    public GetFutureCourtSessionsQuery(DateTimeOffset? referenceTime = null)
    {
        ReferenceTime = referenceTime ?? DateTimeOffset.UtcNow;
    }
}
