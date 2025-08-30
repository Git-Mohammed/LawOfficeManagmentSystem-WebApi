using LOMs.Domain.Common.Results;

namespace LOMs.Domain.People.Clients
{
    public static class ClientErrors
    {
        public static readonly Error PersonRequired =
            Error.Validation("Client_Person_Required", "Client must have valid personal information.");
    }

}
