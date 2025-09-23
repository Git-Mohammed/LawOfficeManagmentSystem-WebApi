using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Domain.Cases;
using LOMs.Domain.Common.Results;
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
            .Include(x => x.Employee).ThenInclude(e => e.Person)
            .Include(x => x.ClientCases).ThenInclude(cc => cc.Client).ThenInclude(c => c.Person)
            .FirstOrDefaultAsync(x => x.Id == request.CaseId, cancellationToken);

        if (caseEntity is null)
        {
            _logger.LogWarning("Case with Id {CaseId} not found.", request.CaseId);
            return CaseErrors.CaseNotFound;
        }

        var createdByEmployee = await _context.Employees
            .AsNoTracking()
            .Where(x => x.UserId.Equals(caseEntity.CreatedBy))
            .Select(x => new
            {
                FullName = x.Person!.FullName,
                CreatedByEmployeeUserId = x.UserId
            }) 
            .FirstOrDefaultAsync(e => e.CreatedByEmployeeUserId == caseEntity.CreatedBy, cancellationToken);

        var dto = new CaseDetailsDto
        {
            CaseId = caseEntity.Id,
            PartyRole = caseEntity.PartyRole,
            CaseNumber = caseEntity.CaseNumber,
            CaseSubject = caseEntity.CaseSubject,
            CourtTypeName = caseEntity.CourtType?.Name ?? string.Empty,
            AssignedEmployeeName = caseEntity.Employee?.Person?.FullName ?? string.Empty,
            Status = caseEntity.Status,
            CreatedAt = caseEntity.CreatedAtUtc,
            CreatedBy = createdByEmployee?.FullName ?? string.Empty,
            ClientRequests = caseEntity.ClientRequests,
            EstimatedReviewDate = caseEntity.EstimatedReviewDate,
            LawyerOpinion = caseEntity.LawyerOpinion,
            Clients = caseEntity.ClientCases
                .Select(cc => new CaseClientDto
                {
                    ClientId = cc.Client!.Id,
                    FullName = cc.Client.Person?.FullName ?? string.Empty,
                    NationalId = cc.Client.Person?.NationalId ?? string.Empty,
                    PhoneNumber = cc.Client.Person?.PhoneNumber ?? string.Empty,
                    CountryCode = cc.Client.Person?.CountryCode.ToString() ?? string.Empty,
                    Address = cc.Client.Person?.Address ?? string.Empty,
                    Birthdate = cc.Client.Person!.BirthDate
                })
                .ToList()
        };

        _logger.LogInformation("Case with Id {CaseId} retrieved successfully.", request.CaseId);
        return dto;
    }

}
