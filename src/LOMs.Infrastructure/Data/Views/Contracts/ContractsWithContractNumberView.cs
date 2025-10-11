using LOMs.Infrastructure.Constants;
using LOMs.Infrastructure.Enums;

namespace LOMs.Infrastructure.Data.Views.Contracts;

internal static class ContractsWithContractNumberView
{
    public static DatabaseSchema DatabaseSchema => DatabaseSchema.Finance;
    public static string Schema => SchemaNames.GetName(DatabaseSchema);
    public const string Name = "vw_ContractsWithContractNumber";

    public static readonly string Sql = @$"
        CREATE OR ALTER VIEW [{Schema}].[{Name}] AS
        SELECT 
            [Id],
            [CaseId],
            [HijriYear],
            [CourtTypeCode],
            CASE [Type]
                WHEN 'FixedTerm' THEN 1
                WHEN 'OpenEnded' THEN 2
                ELSE NULL
            END AS Type,
            [IssuedOn],
            [ExpiresOn],
            [BaseAmount],
            [InitialPayment],
            [TaxAmount],
            [IsAssigned],
            [CreatedAtUtc],
            CAST([CreatedBy] AS NVARCHAR(100)) AS CreatedBy,
            [LastModifiedUtc],
            CAST([LastModifiedBy] AS NVARCHAR(100)) AS LastModifiedBy,
            ROW_NUMBER() OVER (
                PARTITION BY HijriYear, CourtTypeCode
                ORDER BY [CreatedAtUtc]
            ) AS RowNumber,
            CONCAT(
                N'ع',
                RIGHT(CAST(HijriYear AS VARCHAR), 2), '-', 
                CourtTypeCode, '-', 
                FORMAT(
                    ROW_NUMBER() OVER (
                        PARTITION BY HijriYear, CourtTypeCode
                        ORDER BY [CreatedAtUtc]
                    ), '0000'
                )
            ) AS ContractNumber
        FROM [{Schema}].[Contracts];
    ";
}
