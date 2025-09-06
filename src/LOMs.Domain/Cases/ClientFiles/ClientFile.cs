using LOMs.Domain.Cases.CourtTypes;
using LOMs.Domain.Common;
using LOMs.Domain.Common.Results;
using LOMs.Domain.People.Clients;

namespace LOMs.Domain.Cases.ClientFiles
{
    public sealed class ClientFile : AuditableEntity
    {
        public Guid ClientId { get; private set; }
        public string FileNumber { get; private set; } = null!;
        public short HijriYear { get; private set; }
        public short CourtTypeCode { get; private set; }
        public int OrderNumber { get; private set; }

        public Client? Client { get; private set; }
        public ICollection<ClientCase> CaseClients { get; private set; } = new List<ClientCase>();

        private ClientFile() { } // EF Core

        private ClientFile(Guid id, Guid clientId, short courtTypeCode, string fileNumber,
            short hijriYear, int orderNumber) : base(id)
        {
            ClientId = clientId;
            FileNumber = fileNumber;
            HijriYear = hijriYear;
            OrderNumber = orderNumber;
            CourtTypeCode = courtTypeCode;
        }

        /// <summary>
        /// Domain-level factory method for creating a ClientFile.
        /// Assumes that fileNumber, hijriYear, and orderNumber have already been calculated.
        /// </summary>
        public static Result<ClientFile> Create(
            Guid clientId,
            CourtType courtType,
            short hijriYear,
            int orderNumber)
        {
            if (clientId == Guid.Empty)
                return ClientFileErrors.InvalidClientId;

            if (courtType is null)
                return ClientFileErrors.InvalidCourtType;

            if (hijriYear <= 0)
                return ClientFileErrors.InvalidHijriYear;

            if (orderNumber <= 0)
                return ClientFileErrors.InvalidOrderNumber;

            var fileNumber = GenerateClientFileNumber(hijriYear, courtType.Code, orderNumber);

            var clientFile = new ClientFile(
                Guid.NewGuid(),
                clientId,
              courtType.Code,
                fileNumber,
                hijriYear,
                orderNumber
            );

            return clientFile;
        }
        private static string GenerateClientFileNumber(int hijriYear, short courtCode, int orderNumber)
        {
            // Format: YY-CourtCode-Order
            return $"{hijriYear:D2}-{courtCode}-{orderNumber}";
        }
        public override string ToString() => $"ClientFile #{FileNumber}";
    }
}
