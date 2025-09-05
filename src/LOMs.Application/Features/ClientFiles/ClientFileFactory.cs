using System.Globalization;
using LOMs.Domain.Cases.ClientFiles;
using LOMs.Domain.Cases.CourtTypes;
using LOMs.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using LOMs.Domain.Common.Results;

namespace LOMs.Application.Features.ClientFiles
{
    public sealed class ClientFileFactory(IAppDbContext context) : IClientFileFactory
    {
        private readonly IAppDbContext _context = context;

        public async Task<Result<ClientFile>> CreateAsync(Guid clientId, CourtType courtType, CancellationToken ct = default)
        {
            var hijriYear = new HijriCalendar().GetYear(DateTime.UtcNow) % 100;

            var existingCount = await _context.ClientFiles
                .CountAsync(x => x.CourtTypeCode == courtType.Code && x.HijriYear == hijriYear, ct);

            var orderNumber = existingCount + 1;


            var result = ClientFile.Create(clientId, courtType,  (short)hijriYear, orderNumber);

            if (result.IsError)
                return result.Errors;

            return result.Value;
        }

    }

    public interface IClientFileFactory
    {
        Task<Result<ClientFile>> CreateAsync(Guid clientId, CourtType courtType, CancellationToken ct = default);
    }
}
