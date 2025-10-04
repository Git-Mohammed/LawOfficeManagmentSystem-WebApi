using LOMs.Application.Features.Cases.Dtos;
using LOMs.Application.Features.CourtTypes.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.CourtTypes
{
    public interface ICourtTypeService
    {
        Task<Result<List<CourtTypeDto>>> GetAllAsync(CancellationToken ct);

        Task<Result<List<short>>> GetYearsByCourtTypeAsync(Guid courtTypeId, CancellationToken ct);
        Task<Result<List<CourtTypeWithYearsDto>>> GetCourtTypesWithHijriYears(CancellationToken ct);

    }
}
