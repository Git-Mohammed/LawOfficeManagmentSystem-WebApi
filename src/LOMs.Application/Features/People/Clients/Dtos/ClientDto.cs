using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Application.Features.People.Clients.Dtos
{
    public sealed class ClientDto
    {
        public Guid ClientId { get; init; }
        public PersonDto Person { get; init; } = null!;
    }

}
