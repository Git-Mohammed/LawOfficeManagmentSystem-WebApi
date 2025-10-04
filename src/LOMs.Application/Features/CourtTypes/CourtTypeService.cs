using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Cases.Dtos;
using LOMs.Application.Features.CourtTypes.Dtos;
using LOMs.Domain.Cases.CourtTypes;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.CourtTypes;

public class CourtTypeService(
    IAppDbContext context,
    IMapper mapper,
    ILogger<CourtTypeService> logger) : ICourtTypeService
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

            var dtos = mapper.Map<List<CourtType>, List<CourtTypeDto>>(entities);

            return dtos;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve court types.");
            return CourtTypeErrors.FailedToRetrieveCourtTypes;
        }
    }

    public async Task<Result<List<short>>> GetYearsByCourtTypeAsync(Guid courtTypeId, CancellationToken ct)
    {
        try
        {
            var courtType = await _context.CourtTypes
                .FirstOrDefaultAsync(x => x.Id.Equals(courtTypeId), ct);

            if (courtType is null)
                return CourtTypeErrors.NotFound;

            var years = await _context.ClientFiles
                .AsNoTracking()
                .Where(cf => cf.CourtTypeCode == courtType.Code)
                .Select(cf => cf.HijriYear)
                .Distinct()
                .OrderByDescending(y => y)
                .ToListAsync(ct);

            return years;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve Hijri years for CourtTypeId: {CourtTypeId}", courtTypeId);
            return CourtTypeErrors.FailedToRetrieveYears;
        }
    }

    public async Task<Result<List<CourtTypeWithYearsDto>>> GetCourtTypesWithHijriYears(CancellationToken ct)
    {
        var courtTypes = await _context.CourtTypes
            .AsNoTracking()
            .Select(ct => new CourtTypeWithYearsDto
            {
                CourtTypeId = ct.Id,
                CourtTypeName = ct.Name,
                HijriYears = _context.ClientFiles
                    .Where(cf => cf.CourtTypeCode == ct.Code)
                    .Select(cf => cf.HijriYear)
                    .Distinct()
                    .OrderByDescending(y => y)
                    .ToList()
            })
            .ToListAsync(ct);

        if (!courtTypes.Any())
            return CourtTypeErrors.NoCourtTypesFound;

        return courtTypes;
    }
}
