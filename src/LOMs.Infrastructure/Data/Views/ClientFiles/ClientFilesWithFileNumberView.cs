using LOMs.Infrastructure.Constants;
using LOMs.Infrastructure.Enums;

namespace LOMs.Infrastructure.Data.Views.ClientFiles;

internal static class ClientFilesWithFileNumberView
{
    public const string Name = "vw_ClientFilesWithFileNumber";
    public static DatabaseSchema DatabaseSchema => DatabaseSchema.Case;
    public static string Schema => SchemaNames.GetName(DatabaseSchema);

    public static readonly string Sql = @$"
        CREATE OR ALTER VIEW [{Schema}].[{Name}] AS
        SELECT 
            [Id],
            [ClientId],
            [HijriYear],
            [CourtTypeCode],
            [CreatedAtUtc],
            [CreatedBy],
            [LastModifiedUtc],
            [LastModifiedBy],
            ROW_NUMBER() OVER (
                PARTITION BY HijriYear, CourtTypeCode
                ORDER BY [CreatedAtUtc]
            ) AS RowNumber,
            CONCAT(
                RIGHT(CAST(HijriYear AS VARCHAR), 2), '-', 
                CourtTypeCode, '-', 
                FORMAT(
                    ROW_NUMBER() OVER (
                        PARTITION BY HijriYear, CourtTypeCode
                        ORDER BY [CreatedAtUtc]
                    ), '0000'
                )
            ) AS FileNumber
        FROM [{Schema}].[ClientFiles];
    ";
}
