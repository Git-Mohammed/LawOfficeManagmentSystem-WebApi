using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.POAs.Dtos;
using LOMs.Domain.Common.Results;
using LOMs.Domain.POAs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.POAs.Queries.GetById;

public sealed class GetPOAByIdQueryHandler(
    IAppDbContext context,
    ILogger<GetPOAByIdQueryHandler> logger
) : IQueryHandler<GetPOAByIdQuery, Result<POADto>>
{
    public async Task<Result<POADto>> HandleAsync(GetPOAByIdQuery query, CancellationToken cancellationToken = default)
    {
        var poa = await context.POAs
            .Where(p => p.CaseId == query.CaseId && p.Id == query.POAId)
            .Select(p => new POADto
            {
                POAId = p.Id,
                POANumber = p.POANumber,
                IssueDate = p.IssueDate,
                IssuingAuthority = p.IssuingAuthority
              
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (poa is null)
        {
            logger.LogWarning("POA with ID {POAId} not found for Case ID {CaseId}.", query.POAId, query.CaseId);
            return POAErrors.NotFound;
        }

        return poa;
    }
}
