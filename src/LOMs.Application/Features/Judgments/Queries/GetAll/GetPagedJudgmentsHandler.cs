using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Templates.DTOs;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Judgments.Queries.GetAll;

public sealed class GetPagedJudgmentsHandler
    : IQueryHandler<GetPagedJudgmentsQuery, Result<PaginatedList<JudgmentDetailsDto>>>
{
    private readonly IAppDbContext _context;
    private readonly ILogger<GetPagedJudgmentsHandler> _logger;

    public GetPagedJudgmentsHandler(
        IAppDbContext context,
        ILogger<GetPagedJudgmentsHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<PaginatedList<JudgmentDetailsDto>>> HandleAsync(
        GetPagedJudgmentsQuery query,
        CancellationToken ct)
    {
        if (query.PageNumber < 1 || query.PageSize < 1)
        {
            _logger.LogWarning("Invalid pagination parameters: PageNumber={PageNumber}, PageSize={PageSize}", query.PageNumber, query.PageSize);
            return Error.Validation("Pagination.Invalid", "Page number must be ≥ 1 and page size grater than 0.");
        }

        var baseQuery = _context.Judgments.AsNoTracking();

        var totalCount = await baseQuery.CountAsync(ct);

        var judgments = await (
            from j in baseQuery
            join e in _context.Employees.AsNoTracking() on j.CreatedBy equals e.UserId into empGroup
            from emp in empGroup.DefaultIfEmpty()
            join f in _context.JudgmentFiles.AsNoTracking() on j.Id equals f.JudgmentId into fileGroup
            orderby j.CreatedAtUtc descending
            select new JudgmentDetailsDto
            {
                JudgmentId = j.Id,
                CaseId = j.CaseId,
                Type = j.Type,
                SubType = j.SubType,
                Number = j.Number,
                IssuedOn = j.IssuedOn,
                EmployeeIdCreatedBy = emp.Id,
                EmployeeNameCreatedBy = emp.Person.FullName ?? "Unknown",
                CreatedDate = j.CreatedAtUtc,
                Files =fileGroup
                .OrderByDescending(f => f.CreatedAtUtc)
                .Select(
                    f =>new FileDto { 
                        FileName =f.Filename,
                        FilePath=f.Filepath,
                        FileType=f.ContentType}).ToList(),
            })
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(ct);

        return new PaginatedList<JudgmentDetailsDto>(
            judgments, query.PageNumber, query.PageSize, totalCount
        );
    }
}
