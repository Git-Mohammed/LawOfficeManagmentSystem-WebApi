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

        public Result<Contract> CreateAsync(
            Guid caseId,
            CourtType courtType,
            ContractType type,
            DateOnly? issuedOn,
            DateOnly? expiresOn,
            decimal totalAmount,
            decimal initialPayment,
            bool isAssigned,
            CancellationToken ct = default)
        {

            var hijriYear = HijriDateConverter.GetFullHijriYear();

            var result = Contract.Create(
                caseId,
                courtType,
                type,
                issuedOn,
                expiresOn,
                totalAmount,
                initialPayment,
                isAssigned,
                (short)hijriYear
            );

            if (result.IsError)
                return result.Errors;

            return result.Value;
        }
    }
}
