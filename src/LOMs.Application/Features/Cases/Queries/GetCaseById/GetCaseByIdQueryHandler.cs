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
            .Where(x => x.Id == request.CaseId)
            .Select(x => new CaseDetailsDto
            {
                CaseId = x.Id,
                CaseNumber = x.CaseNumber,
                CaseSubject = x.CaseSubject,
                CourtTypeName = x.CourtType.Name,
                AssignedEmployeeName = x.AssignedEmployeeId != null ? x.Employee.Person.FullName : string.Empty,
                Status = x.Status,
                Clients = x.ClientCases
                    .Select(cc => new CaseClientDto
                    {
                        ClientId = cc.Client.Id,
                        FullName = cc.Client.Person.FullName ?? string.Empty,
                        NationalId = cc.Client.Person.NationalId ?? string.Empty,
                        PhoneNumber = cc.Client.Person.PhoneNumber ?? string.Empty,
                        CountryCode = cc.Client.Person.CountryCode.ToString() ?? string.Empty,
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (caseEntity is null)
        {
            _logger.LogWarning("Case with Id {CaseId} not found.", request.CaseId);
            return CaseErrors.CaseNotFound;
        }

        _logger.LogInformation("Case with Id {CaseId} retrieved successfully.", request.CaseId);

        return caseEntity;
    }
}
