using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Cases.Dtos;
using LOMs.Application.Features.Cases.Queries.CaseListRow;
using LOMs.Domain.Common.Results;
using LOMs.Domain.Cases.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Cases.Queries.GetCasesList;

public class GetCasesListQueryHandler
    : IQueryHandler<GetCasesListQuery, Result<PaginatedList<CaseListRowDto>>>
{
    private readonly ILogger<GetCasesListQueryHandler> _logger;
    private readonly IAppDbContext _context;

    public GetCasesListQueryHandler(
        ILogger<GetCasesListQueryHandler> logger,
        IAppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<PaginatedList<CaseListRowDto>>> HandleAsync(
        GetCasesListQuery query,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Fetching case list. Page: {PageNumber}, Size: {PageSize}, Status: {Status}",
            query.PageNumber, query.PageSize, query.Status?.ToString() ?? "Any");
        var baseQuery =
            from caseEntity in _context.Cases.AsNoTracking()
            from clientCase in caseEntity.ClientCases
            join fileView in _context.ClientFilesWithFileNumber
                on clientCase.ClientFileId equals fileView.Id
            join employee in _context.Employees.AsNoTracking()
                on caseEntity.CreatedBy equals employee.UserId into employeeGroup
            from employee in employeeGroup.DefaultIfEmpty()
            select new CaseListRowDto
            {
                CaseId = caseEntity.Id,
                CaseNumber = caseEntity.CaseNumber ?? "N/A",
                FileNumber = fileView.FileNumber ?? "N/A",
                Status = caseEntity.Status,
                CreatedDate = caseEntity.CreatedAtUtc,
                CaseSubject = caseEntity.CaseSubject ?? "N/A",
                CourtTypeName = caseEntity.CourtType!.Name,
                CreatedByName = employee != null ? employee.Person!.FullName : "N/A"
            };


        if (query.Status is not null)
        {
            baseQuery = baseQuery.Where(c => c.Status == query.Status);
        }

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        var items = await baseQuery
            .OrderByDescending(c => c.CreatedDate)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        var paginatedResult = new PaginatedList<CaseListRowDto>(
            items, query.PageNumber, query.PageSize, totalCount);

        return paginatedResult;
    }
}
