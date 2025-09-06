using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Cases.Dtos;
using LOMs.Domain.Common.Results;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LOMs.Domain.Cases.CourtTypes;

namespace LOMs.Application.Features.CourtTypes;

public class CourtTypeService(IAppDbContext context, IMapper mapper, ILogger<CourtTypeService> logger) : ICourtTypeService
{
    private readonly IAppDbContext _context = context;

    public async Task<Result<List<CourtTypeDto>>> GetAllAsync(CancellationToken ct)
    {
        try
        {
            var entities = await _context.CourtTypes
                .AsNoTracking()
                .OrderBy(c => c.Code)
                .ToListAsync(ct);

            var dtos = mapper.Map<List<CourtType>,List<CourtTypeDto>>(entities);

            return dtos;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve court types.");
            return CourtTypeErrors.FailedToRetrieveCourtTypes;
        }
    }
}
