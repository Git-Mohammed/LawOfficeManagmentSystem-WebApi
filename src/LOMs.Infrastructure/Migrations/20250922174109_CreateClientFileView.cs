using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOMs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateClientFileView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Case') EXEC('CREATE SCHEMA [Case]');");

            migrationBuilder.Sql(@"
        CREATE VIEW [Case].[vw_ClientFilesWithFileNumber] AS
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
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW [Case].[vw_ClientFilesWithFileNumber];");
        }
    }
}
