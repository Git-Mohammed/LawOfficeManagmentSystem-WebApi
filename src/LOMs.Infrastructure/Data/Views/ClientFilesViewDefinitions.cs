namespace LOMs.Infrastructure.Data.Views;

internal static class ClientFilesViewDefinitions
{
    public const string ClientFilesWithFileNumber = @"
        CREATE OR ALTER VIEW [Case].[vw_ClientFilesWithFileNumber] AS
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
        FROM [Case].[ClientFiles];
    ";
}
