using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Attachments.DTOs;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Attachments.Queries.GetAttachmentsByCaseId.Paged;

/// <summary>
/// Query to retrieve a paginated list of attachments for a given CaseId.
/// </summary>
public sealed record GetPagedAttachmentsByCaseIdQuery(
    Guid CaseId,
    int PageNumber,
    int PageSize
) : IQuery<Result<PaginatedList<AttachmentDto>>>;
