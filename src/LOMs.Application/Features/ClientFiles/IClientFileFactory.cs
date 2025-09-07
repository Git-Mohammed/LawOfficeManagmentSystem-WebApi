using LOMs.Domain.Cases.ClientFiles;
using LOMs.Domain.Cases.CourtTypes;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.ClientFiles
{
    public interface IClientFileFactory
    {
        Task<Result<ClientFile>> CreateAsync(Guid clientId, CourtType courtType, CancellationToken ct = default);
    }
}
