using LOMs.Infrastructure.Enums;

namespace LOMs.Infrastructure.Constants;

public static class SchemaNames
{
    private const string Identity = "Identity";
    private const string User = "UserManagment";
    private const string Case = "CaseManagment";
    private const string Finance = "FinanceManagment";
    private const string File = "FileManagment";
    private const string Templates = "Templates";
    public static string GetName(DatabaseSchema schema) => schema switch
    {
        DatabaseSchema.Identity => Identity,
        DatabaseSchema.User => User,
        DatabaseSchema.Case => Case,
        DatabaseSchema.Finance => Finance,
        DatabaseSchema.File => File,
        DatabaseSchema.Templates => Templates,
        _ => throw new ArgumentOutOfRangeException(nameof(schema), $"Unknown schema: {schema}")
    };
}
