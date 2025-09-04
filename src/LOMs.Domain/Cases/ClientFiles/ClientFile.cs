using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Common;
using LOMs.Domain.Common.Results;
using LOMs.Domain.People.Clients;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace LOMs.Domain.Cases.ClientFiles
{
    public sealed class ClientFile : AuditableEntity
    {
        public Guid ClientId { get; }
        public string FileNumber { get; private set; } = null!;

        /// <summary>
        /// Extracted CourtType from FileNumber. If parsing fails, defaults to CourtType.Other.
        /// </summary>
        [NotMapped]
        public CourtType CourtType => TryParseCourtTypeFromFileNumber(FileNumber);

        public ICollection<ClientCase> CaseClients { get; private set; } = new List<ClientCase>();
        public Client? Client { get; set; }

        private ClientFile() { }

        private ClientFile(Guid id, Guid clientId, string fileNumber) : base(id)
        {
            ClientId = clientId;
            FileNumber = fileNumber;
        }

        /// <summary>
        /// Factory method to create a new ClientFile with validation and file number generation.
        /// </summary>
        public static Result<ClientFile> Create(Guid id, Guid clientId, CourtType courtType)
        {
            if (id == Guid.Empty)
                return Error.Conflict();

            if (clientId == Guid.Empty)
                return ClientFileErrors.InvalidClientId;

            var fileNumber = GenerateFileNumber(id, courtType);
            return new ClientFile(id, clientId, fileNumber);
        }

        /// <summary>
        /// Generates a file number in the format: YY-CourtTypeId-XXXXXX
        /// </summary>
        private static string GenerateFileNumber(Guid id, CourtType courtType)
        {
            var hijriCalendar = new HijriCalendar();
            var hijriYear = hijriCalendar.GetYear(DateTime.UtcNow) % 100;
            var guidSegment = id.ToString("N")[..6];

            return $"{hijriYear:D2}-{(int)courtType}-{guidSegment}";
        }

        /// <summary>
        /// Attempts to parse CourtType from a given file number string.
        /// </summary>
        private static CourtType TryParseCourtTypeFromFileNumber(string? fileNumber)
        {
            if (string.IsNullOrWhiteSpace(fileNumber))
                return CourtType.Other;

            var parts = fileNumber.Split('-');
            if (parts.Length < 2 || !int.TryParse(parts[1], out var courtTypeId))
                return CourtType.Other;

            return Enum.IsDefined(typeof(CourtType), courtTypeId)
                ? (CourtType)courtTypeId
                : CourtType.Other;
        }

        public override string ToString() => $"File #{FileNumber} ({CourtType})";
    }
}
