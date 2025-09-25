using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.POAs.Queries.DownloadPoaFile;

public record DownloadPoaFileQuery(Guid CaseId, Guid POAId, string RelativePath)
    : IQuery<Result<FileDownloadResult>>;
