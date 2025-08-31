using LOMs.Domain.Cases.Enums.CourtTypes;
using LOMs.Domain.Common;
using LOMs.Domain.Common.Results;
using LOMs.Domain.People.Clients;
using System.Globalization;

namespace LOMs.Domain.Cases.ClientFiles
{
    public sealed class ClientFile : AuditableEntity
    {
        public Guid ClientId { get; }
        public string FileNumber { get; private set; } = null!;

        private readonly List<Case> _cases = [];
        public IEnumerable<Case> Cases => _cases.AsReadOnly();

        public Client? Client { get; set; }

        private ClientFile()
        {

        }

        private ClientFile(Guid id, Guid clientId, string fileNumber) : base(id)
        {
            ClientId = clientId;
            FileNumber = fileNumber;
        }

        public static Result<ClientFile> Create(Guid id, Guid clientId, CourtType courtType)
        {
            if (id == Guid.Empty)
                return Error.Conflict();
            if (clientId == Guid.Empty)
                return ClientFileErrors.InvalidClientId;

            var fileNumber = _GenerateFileNumber(id, courtType);

            return new ClientFile(id, clientId, fileNumber);
        }

        private static string _GenerateFileNumber(Guid id,CourtType courtType)
        {
            var hijriCalendar = new HijriCalendar();
            var hijriYear = hijriCalendar.GetYear(DateTime.Now) % 100; // e.g., 1447 → 47

            return $"{hijriCalendar:D2}-{(int)courtType}-{id.ToString("N").Substring(0,6)}";
        }

    }
}
