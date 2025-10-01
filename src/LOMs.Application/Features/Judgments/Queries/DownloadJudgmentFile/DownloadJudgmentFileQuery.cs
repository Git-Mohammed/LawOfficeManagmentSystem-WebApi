using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Judgments.Queries.DownloadJudgmentFile;

public record DownloadJudgmentFileQuery(Guid CaseId, Guid JudgmentId, string RelativePath)
    : IQuery<Result<FileDownloadResult>>;
