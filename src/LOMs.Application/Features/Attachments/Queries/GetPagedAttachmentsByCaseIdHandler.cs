using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Attachments.DTOs;
using LOMs.Application.Features.Templates.DTOs;
using LOMs.Domain.Cases;
using LOMs.Domain.Cases.Attachments;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Attachments.Queries.GetAttachmentsByCaseId.Paged;

public sealed class GetPagedAttachmentsByCaseIdHandler
    : IQueryHandler<GetPagedAttachmentsByCaseIdQuery, Result<PaginatedList<AttachmentDto>>>
{
    private readonly IAppDbContext _context;
    private readonly ILogger<GetPagedAttachmentsByCaseIdHandler> _logger;

    public GetPagedAttachmentsByCaseIdHandler(
        IAppDbContext context,
        ILogger<GetPagedAttachmentsByCaseIdHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<PaginatedList<AttachmentDto>>> HandleAsync(
        GetPagedAttachmentsByCaseIdQuery query,
        CancellationToken ct)
    {
        // Validate pagination
        if (query.PageNumber < 1)
        {
            _logger.LogWarning("Invalid page number: {PageNumber}", query.PageNumber);
            return Error.Validation("Pagination.InvalidPageNumber", "Page number must be at least 1.");
        }

        if (query.PageSize < 1 || query.PageSize > 100)
        {
            _logger.LogWarning("Invalid page size: {PageSize}", query.PageSize);
            return Error.Validation("Pagination.InvalidPageSize", "Page size must be between 1 and 100.");
        }

        // Ensure case exists
        var caseExists = await _context.Cases
            .AsNoTracking()
            .AnyAsync(c => c.Id == query.CaseId, ct);

        if (!caseExists)
        {
            _logger.LogWarning("Fetching attachments failed: Case not found. CaseId: {CaseId}", query.CaseId);
            return CaseErrors.CaseNotFound;
        }

        // Build query
        var attachmentsQuery = _context.Attachments
            .AsNoTracking()
            .Where(a => a.CaseId == query.CaseId);

        var totalCount = await attachmentsQuery.CountAsync(ct);

        var attachments = await (
            from attachment in attachmentsQuery
            join employee in _context.Employees.AsNoTracking()
                on attachment.CreatedBy equals employee.UserId into empGroup
            from emp in empGroup.DefaultIfEmpty()
            orderby attachment.CreatedAtUtc descending
            select new AttachmentDto
            {
                AttachmentId = attachment.Id,
                AttachmentName = attachment.AttachmentName ?? string.Empty,
                CreatedDate = attachment.CreatedAtUtc,
                CreatedByEmployeeName = emp.Person!.FullName ?? string.Empty,
                File = new FileDto
                {
                    FilePath = attachment.Filepath ?? string.Empty,
                    FileName = attachment.Filename ?? string.Empty,
                    FileType = attachment.ContentType ?? string.Empty
                }
            })
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(ct);

        if (!attachments.Any())
        {
            _logger.LogWarning("No attachments found for CaseId {CaseId}", query.CaseId);
            return AttachmentErrors.NotFound;
        }

        var paginated = new PaginatedList<AttachmentDto>(
            items: attachments,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            totalItemsCount: totalCount
        );

        _logger.LogInformation("Retrieved {Count} attachments for CaseId {CaseId}", attachments.Count, query.CaseId);

        return paginated;
    }
}
