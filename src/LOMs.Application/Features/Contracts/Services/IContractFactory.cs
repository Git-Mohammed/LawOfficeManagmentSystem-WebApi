using LOMs.Domain.Cases.Contracts;
using LOMs.Domain.Cases.CourtTypes;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.Contracts.Services
{
    public interface IContractFactory
    {
        Task<Result<Contract>> CreateAsync(
            Guid caseId,
            CourtType courtType,
            ContractType type,
            DateOnly? issuedOn,
            DateOnly? expiresOn,
            decimal totalAmount,
            decimal initialPayment,
            bool isAssigned,
            List<ContractFile> contractFiles,
            CancellationToken ct = default);
    }
}
