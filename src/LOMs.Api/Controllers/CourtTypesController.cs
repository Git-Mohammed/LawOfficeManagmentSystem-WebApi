using LOMs.Application.Features.Cases.Dtos;
using LOMs.Application.Features.CourtTypes;
using Microsoft.AspNetCore.Mvc;

namespace LOMs.Api.Controllers;

[Route("api/court-types")]
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
}
