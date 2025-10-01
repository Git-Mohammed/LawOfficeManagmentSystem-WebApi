using LOMs.Infrastructure.Data.Views.ClientFiles;
using LOMs.Infrastructure.Data.Views.Contracts;
using LOMs.Infrastructure.Enums;

namespace LOMs.Infrastructure.Data.Views.Registry;

public static class SqlViewRegistry
{
    public static readonly IReadOnlyList<SqlViewDefinition> All = new[]
    {
        new SqlViewDefinition(
            ClientFilesWithFileNumberView.DatabaseSchema,
            ClientFilesWithFileNumberView.Name,
            ClientFilesWithFileNumberView.Sql
        ),
        new SqlViewDefinition(
            ContractsWithContractNumberView.DatabaseSchema,
            ContractsWithContractNumberView.Name,
            ContractsWithContractNumberView.Sql
        )


        // Add more views here as needed
    };
}
