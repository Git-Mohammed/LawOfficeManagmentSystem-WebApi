using System;
using System.Threading;
using System.Threading.Tasks;
using LOMs.Domain.Cases.ClientFiles;
using LOMs.Domain.Cases.CourtTypes;
using LOMs.Application.Common.Interfaces;
using LOMs.Domain.Common.Results;
using LOMs.Application.Utilities;

namespace LOMs.Application.Features.ClientFiles
{
    public sealed class ClientFileFactory : IClientFileFactory
    {
        private readonly IAppDbContext _context;

        public ClientFileFactory(IAppDbContext context)
        {
            _context = context;
        }

        public  Result<ClientFile> CreateAsync(Guid clientId, CourtType courtType, CancellationToken ct = default)
        {
            var hijriYear = HijriDateConverter.GetFullHijriYear();

            var result = ClientFile.Create(clientId, courtType, (short)hijriYear);

            return result.IsError ? result.Errors : result.Value;
        }
    }
}
