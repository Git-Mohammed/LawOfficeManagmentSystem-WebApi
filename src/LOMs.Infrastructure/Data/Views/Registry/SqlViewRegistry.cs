using LOMs.Infrastructure.Data.Views.ClientFiles;
using LOMs.Infrastructure.Enums;

namespace LOMs.Infrastructure.Data.Views.Registry;

public static class SqlViewRegistry
{
    public static readonly IReadOnlyList<SqlViewDefinition> All = new[]
    {
        new SqlViewDefinition(
            DatabaseSchema.Case,
            ClientFilesWithFileNumberView.Name,
            ClientFilesWithFileNumberView.Sql
        )

        // Add more views here as needed
    };
}
