using LOMs.Domain.Common.Results;

namespace LOMs.Domain.People.Clients;

public static class ClientErrors
{
    public static readonly Error PersonRequired =
        Error.Validation("Client.PersonRequired", "العميل يجب أن يحتوي على بيانات شخصية صحيحة.");
}
