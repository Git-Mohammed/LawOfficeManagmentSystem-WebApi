using LiteBus.Queries.Abstractions;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Contracts.Queries.DownloadContractFile;
public record DownloadContractFileQuery(Guid CaseId, Guid ContractId, string RelativePath)
    : IQuery<Result<FileDownloadResult>>;
