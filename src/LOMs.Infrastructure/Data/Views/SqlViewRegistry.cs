using LOMs.Infrastructure.Enums;
using LOMs.Infrastructure.Data.Views;

namespace LOMs.Infrastructure.Data;

public static class SqlViewRegistry
{
    public static readonly IReadOnlyList<SqlViewDefinition> All = new[]
    {
        new SqlViewDefinition(
            DatabaseSchema.Case,
            "vw_ClientFilesWithFileNumber",
            ClientFilesViewDefinitions.ClientFilesWithFileNumber
        )

        // Add more views here as needed
    };
}
