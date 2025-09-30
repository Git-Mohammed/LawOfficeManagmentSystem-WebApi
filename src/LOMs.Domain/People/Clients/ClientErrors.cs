using LOMs.Domain.Common.Results;

namespace LOMs.Domain.People.Clients;

public static class ClientErrors
{
    public static Error Client_NotFoundWithId(Guid clientId) =>
    Error.NotFound("Case_ClientNotFoundWithId", $"Client not found with ID: {clientId}");

    public static Error Client_NotFoundWithNationalId(string NationalId) =>
Error.NotFound("Case_ClientNotFoundWithNationalId", $"Client not found with National ID: {NationalId}");


    public static readonly Error PersonRequired =
        Error.Validation("Client.PersonRequired", "Client must contain valid personal data.");
}
