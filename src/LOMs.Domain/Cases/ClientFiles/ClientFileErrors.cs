using LOMs.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Domain.Cases.ClientFiles
{
    public static class ClientFileErrors
    {
        public static Error InvalidClientId => Error.Conflict("ClientFile_Client_InvalidId", "The client id is invalid");
    }
}
