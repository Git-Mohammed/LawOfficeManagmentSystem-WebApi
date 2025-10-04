using LOMs.Application.Features.Cases.Dtos;
using LOMs.Application.Features.CourtTypes;
using LOMs.Application.Features.CourtTypes.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LOMs.Api.Controllers;

[Route("api/court-types")]
[Authorize]
public class CourtTypesController(ICourtTypeService courtTypeService) : ApiController
{
    private readonly ICourtTypeService _courtTypeService = courtTypeService;

    [HttpGet(Name = nameof(GetAllCourtTypes))]
    [ProducesResponseType(typeof(List<CourtTypeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves all court types.")]
    [EndpointDescription("Returns a list of available court types for use in dropdowns, filtering, or case classification.")]
    [EndpointName(nameof(GetAllCourtTypes))]
    public async Task<IActionResult> GetAllCourtTypes(CancellationToken ct)
    {
        var result = await _courtTypeService.GetAllAsync(ct);
        return result.Match(Ok, Problem);
    }

    [HttpGet("{courtTypeId:guid}/years")]
    [ProducesResponseType(typeof(List<short>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves Hijri years for a specific court type.")]
    [EndpointDescription("Returns a distinct list of Hijri years associated with the given court type, ordered descending.")]
    [EndpointName(nameof(GetHijriYearsByCourtTypeById))]
    public async Task<IActionResult> GetHijriYearsByCourtTypeById(Guid courtTypeId, CancellationToken ct)
    {
        var result = await _courtTypeService.GetYearsByCourtTypeAsync(courtTypeId, ct);
        return result.Match(Ok, Problem);
    }

[HttpGet("with-hijri-years")]
[ProducesResponseType(typeof(List<CourtTypeWithYearsDto>), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
[EndpointSummary("Retrieves all court types with their associated Hijri years.")]
[EndpointDescription("Returns a list of court types, each including its distinct Hijri years from client files.")]
[EndpointName(nameof(GetCourtTypesWithHijriYears))]
public async Task<IActionResult> GetCourtTypesWithHijriYears(CancellationToken ct)
{
    var result = await _courtTypeService.GetCourtTypesWithHijriYears(ct);
    return result.Match(Ok, Problem);
}

}
