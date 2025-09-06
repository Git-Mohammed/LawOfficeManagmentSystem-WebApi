using LOMs.Domain.Common.Results;

namespace LOMs.Domain.People.Clients;

public static class ClientErrors
{
    public static Error Client_NotFoundWithId(Guid clientId) =>
    Error.NotFound("Case_ClientNotFoundWithId", $"لم يتم العثور على العميل بالمعرّف: {clientId}");

    public static Error Client_NotFoundWithNationalId(string NationalId) =>
Error.NotFound("Case_ClientNotFoundWithNationalId", $"لم يتم العثور على العميل برقم الهوية: {NationalId}");


    public static readonly Error PersonRequired =
        Error.Validation("Client.PersonRequired", "العميل يجب أن يحتوي على بيانات شخصية صحيحة.");
}
