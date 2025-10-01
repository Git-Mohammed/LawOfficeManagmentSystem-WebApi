using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Attachments.Queries.DownloadAttachmentFile
{
    public record class DownloadAttachmentFileQuery(Guid CaseId, Guid AttachmentId, string FilePath):IQuery<Result<FileDownloadResult>>;
}
