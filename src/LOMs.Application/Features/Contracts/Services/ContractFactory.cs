using LOMs.Application.Common.Interfaces;
using LOMs.Application.Utilities;
using LOMs.Domain.Cases.Contracts;
using LOMs.Domain.Cases.CourtTypes;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace LOMs.Application.Features.Contracts.Services
{

    public sealed class ContractFactory(IAppDbContext context) : IContractFactory
    {
        private readonly IAppDbContext _context = context;

        public async Task<Result<Contract>> CreateAsync(
            Guid caseId,
            CourtType courtType,
            ContractType type,
            DateOnly? issuedOn,
            DateOnly? expiresOn,
            decimal totalAmount,
            decimal initialPayment,
            string filePath,
            bool isAssigned,
            CancellationToken ct = default)
        {

            var hijriYear = HijriDateConverter.GetFullHijriYear();

            var existingCount = await _context.Contracts
                .CountAsync(c => c.CourtTypeCode == courtType.Code && c.HijriYear == hijriYear,
                    ct);

            var orderNumber = existingCount + 1;

            var result = Contract.Create(
                caseId,
                courtType,
                type,
                issuedOn,
                expiresOn,
                totalAmount,
                initialPayment,
                filePath,
                isAssigned,
                (short)hijriYear,
                orderNumber
            );

            if (result.IsError)
                return result.Errors;

            return result.Value;
        }
    }
}
