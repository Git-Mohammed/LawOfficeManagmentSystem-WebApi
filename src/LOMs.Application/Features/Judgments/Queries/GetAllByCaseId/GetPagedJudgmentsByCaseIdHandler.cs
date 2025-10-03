using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Judgments.Dtos;
using LOMs.Application.Features.Templates.DTOs;
using LOMs.Domain.Cases;
using LOMs.Domain.Common.Files;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LOMs.Application.Features.Judgments.Queries.GetAllByCaseId
{
    public sealed class GetPagedJudgmentsByCaseIdHandler
        : IQueryHandler<GetPagedJudgmentsByCaseIdQuery, Result<PaginatedList<JudgmentDetailsDto>>>
    {
        private readonly IAppDbContext _context;
        private readonly ILogger<GetPagedJudgmentsByCaseIdHandler> _logger;

        public GetPagedJudgmentsByCaseIdHandler(
            IAppDbContext context,
            ILogger<GetPagedJudgmentsByCaseIdHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<PaginatedList<JudgmentDetailsDto>>> HandleAsync(
            GetPagedJudgmentsByCaseIdQuery query,
            CancellationToken ct)
        {
            var caseExists = await _context.Cases
                .AsNoTracking()
                .AnyAsync(x => x.Id == query.CaseId, ct);

            if (!caseExists)
            {
                _logger.LogWarning("Judgment fetch failed: Case not found. CaseId: {CaseId}", query.CaseId);
                return CaseErrors.CaseNotFound;
            }

            var baseQuery = _context.Judgments
                .AsNoTracking()
                .Where(j => j.CaseId == query.CaseId);

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
                    Number = j.Number ?? string.Empty,
                    IssuedOn = j.IssuedOn,
                    EmployeeIdCreatedBy = emp.Id,
                    EmployeeNameCreatedBy = emp.Person!.FullName ?? "Unknown",
                    CreatedDate = j.CreatedAtUtc,
                    File = fileGroup.Select(x => new FileDto 
                    {
                        FileName = x.Filename ?? string.Empty,
                        FilePath = x.Filepath ?? string.Empty,
                        FileType = x.ContentType ?? string.Empty
                    }).FirstOrDefault()!
                })
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(ct);

            return new PaginatedList<JudgmentDetailsDto>(
                judgments, query.PageNumber, query.PageSize, totalCount
            );
        }
    }
}
