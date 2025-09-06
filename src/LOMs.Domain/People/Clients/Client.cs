using LOMs.Domain.Cases;
using LOMs.Domain.Cases.ClientFiles;
using LOMs.Domain.Common;
using LOMs.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Domain.People.Clients
{
    public sealed class Client : AuditableEntity
    {
        public Guid PersonId { get; }
        public Person? Person { get;  set; }
        private readonly List<ClientFile> _clientFiles = [];
        public IEnumerable<ClientFile> ClientFiles => _clientFiles.AsReadOnly();
        public ICollection<ClientCase> CaseClients { get; private set; } = new List<ClientCase>();
        private Client()
        {
            
        }

        private Client(Guid id, Person person) : base(id)
        {
            Person = person;
        }

        public static Result<Client> Create(Guid id, Person person)
        {
            if (person is null)
                return ClientErrors.PersonRequired;

            return new Client(id, person);
        }
    }
}
