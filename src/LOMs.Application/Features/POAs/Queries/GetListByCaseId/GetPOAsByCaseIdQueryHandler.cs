using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.POAs.Dtos;
using LOMs.Application.Features.Templates.DTOs;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.POAs.Queries.GetListByCaseId;

public sealed class GetPOAsByCaseIdQueryHandler
    : IQueryHandler<GetPOAsByCaseIdQuery, Result<PaginatedList<POAListRowDto>>>
{
    private readonly ILogger<GetPOAsByCaseIdQueryHandler> _logger;
    private readonly IAppDbContext _context;

    public GetPOAsByCaseIdQueryHandler(
        ILogger<GetPOAsByCaseIdQueryHandler> logger,
        IAppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<PaginatedList<POAListRowDto>>> HandleAsync(
        GetPOAsByCaseIdQuery query,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Fetching POAs for CaseId: {CaseId}, Page: {PageNumber}, Size: {PageSize}",
            query.CaseId, query.PageNumber, query.PageSize);

        if (query.PageNumber <= 0 || query.PageSize <= 0)
            return Error.Validation("Page number and size must be greater than zero.");

        var baseQuery =
            from poa in _context.POAs.AsNoTracking()
            where poa.CaseId == query.CaseId
            join employee in _context.Employees.AsNoTracking()
                on poa.CreatedBy equals employee.UserId into empGroup
            from employee in empGroup.DefaultIfEmpty()
            select new POAListRowDto
            {
                POAId = poa.Id,
                CaseId = poa.CaseId,
                POANumber = poa.POANumber,
                IssueDate = poa.IssueDate,
                IssuingAuthority = poa.IssuingAuthority,
                EmployeeNameCreatedBy = employee != null ? employee.Person!.FullName : "N/A",
                CreatedDate = poa.CreatedAtUtc,
                File = poa.POAFiles
            .OrderByDescending(f => f.CreatedAtUtc)
            .Select(f => new FileDto
            {
                FilePath = f.Filepath ?? string.Empty,
                FileName = f.Filename ?? string.Empty,
                FileType = f.ContentType ?? string.Empty
            }).FirstOrDefault()!,
                EmployeeIdCreatedBy = employee != null ? employee.Id : Guid.Empty
            };

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        var items = await baseQuery
            .OrderByDescending(p => p.IssueDate)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        var paginatedResult = new PaginatedList<POAListRowDto>(
            items, query.PageNumber, query.PageSize, totalCount);

        return paginatedResult;
    }
}
