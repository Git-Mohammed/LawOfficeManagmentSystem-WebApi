using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Cases.Dtos;
using LOMs.Domain.Cases;
using LOMs.Domain.Common.Results;
using LOMs.Domain.People.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Cases.Queries.GetCaseByIdQuery;

public sealed class GetCaseByIdQueryHandler(
    ILogger<GetCaseByIdQueryHandler> logger,
    IAppDbContext context,
    IMapper mapper)
    : IQueryHandler<GetCaseByIdQuery, Result<CaseDetailsDto>>
{
    private readonly ILogger<GetCaseByIdQueryHandler> _logger = logger;
    private readonly IAppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<CaseDetailsDto>> HandleAsync(GetCaseByIdQuery request, CancellationToken cancellationToken)
    {
        var caseEntity = await _context.Cases
            .AsNoTracking()
            .Include(x => x.CourtType)
            .Include(c => c.Contracts)
            .Include(c => c.ClientCases)
            .ThenInclude(c => c.Client)
            .ThenInclude(c => c.Person)
            .Include(c => c.Employee)
            .ThenInclude(c => c.Person)
            .Include(c => c.POAs)
            .FirstOrDefaultAsync(c => c.Id == request.CaseId, cancellationToken);

        if (caseEntity is null)
        {
            _logger.LogWarning("Case with Id {CaseId} not found.", request.CaseId);
            return Error.NotFound();
        }

        _logger.LogInformation("Case with Id {CaseId} retrieved successfully.", request.CaseId);

        return _mapper.Map<Case, CaseDetailsDto>(caseEntity);
    }
}
