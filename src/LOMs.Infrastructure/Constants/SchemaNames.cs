using LOMs.Infrastructure.Enums;

namespace LOMs.Infrastructure.Constants;

public static class SchemaNames
{
    public const string Identity = "Identity";
    public const string User = "UserManagment";
    public const string Case = "CaseManagment";
    public const string Finance = "FinanceManagment";
    public const string File = "FileManagment";

    public static string GetName(DatabaseSchema schema) => schema switch
    {
        DatabaseSchema.Identity => Identity,
        DatabaseSchema.User => User,
        DatabaseSchema.Case => Case,
        DatabaseSchema.Finance => Finance,
        DatabaseSchema.File => File,
        _ => throw new ArgumentOutOfRangeException(nameof(schema), $"Unknown schema: {schema}")
    };
}
