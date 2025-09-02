using LOMs.Domain.Cases.ClientFiles;
using LOMs.Domain.People.Clients;

namespace LOMs.Domain.Cases
{
    public sealed class ClientCase
    {
        public Guid CaseId { get; private set; }
        public Guid ClientId { get; private set; }
        public Guid ClientFileId { get; private set; }

        public Case? Case { get;  set; } 
        public Client? Client { get;  set; }
        public ClientFile? ClientFile { get;  set; } 


        private ClientCase() { }

        public ClientCase(Guid caseId, Guid clientId, Guid clientFileId)
        {
            CaseId = caseId;
            ClientId = clientId;
            ClientFileId = clientFileId;
        }
    }
}
