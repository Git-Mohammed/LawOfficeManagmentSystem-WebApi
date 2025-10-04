using LOMs.Domain.Cases.CourtTypes;
using LOMs.Domain.Common;
using LOMs.Domain.Common.Results;
using LOMs.Domain.People.Clients;

namespace LOMs.Domain.Cases.ClientFiles;

public sealed class ClientFile : AuditableEntity
{
    public Guid ClientId { get; private set; }
    public short HijriYear { get; private set; }
    public short CourtTypeCode { get; private set; }

    public Client? Client { get; private set; }
    public ICollection<ClientCase> CaseClients { get; private set; } = new List<ClientCase>();

    private ClientFile() { } // EF Core

    private ClientFile(Guid id, Guid clientId, short courtTypeCode, short hijriYear) : base(id)
    {
        ClientId = clientId;
        HijriYear = hijriYear;
        CourtTypeCode = courtTypeCode;
    }

    /// <summary>
    /// Domain-level factory method for creating a ClientFile.
    /// Assumes that HijriYear and CourtTypeCode are validated.
    /// </summary>
    public static Result<ClientFile> Create(Guid clientId, CourtType courtType, short hijriYear)
    {
        if (clientId == Guid.Empty)
            return ClientFileErrors.InvalidClientId;

        if (courtType is null)
            return ClientFileErrors.InvalidCourtType;

        if (hijriYear <= 0)
            return ClientFileErrors.InvalidHijriYear;

        var clientFile = new ClientFile(
            Guid.NewGuid(),
            clientId,
            courtType.Code,
            hijriYear
        );

        return clientFile;
    }

    public override string ToString() => $"ClientFile for HijriYear {HijriYear}, CourtType {CourtTypeCode}";
}
