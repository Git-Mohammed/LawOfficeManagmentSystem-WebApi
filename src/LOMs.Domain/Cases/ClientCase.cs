using LOMs.Domain.People.Clients;

namespace LOMs.Domain.Cases
{
    public sealed class ClientCase
    {
        public Guid CaseId { get; private set; }
        public Guid ClientId { get; private set; }

        public Case Case { get; private set; } = null!;
        public Client Client { get; private set; } = null!;

        private ClientCase() { }

        public ClientCase(Guid caseId, Guid clientId)
        {
            CaseId = caseId;
            ClientId = clientId;
        }
    }
}
