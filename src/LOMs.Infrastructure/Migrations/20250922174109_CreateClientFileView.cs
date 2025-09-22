using LOMs.Domain.Cases.ClientFiles;
using LOMs.Infrastructure.Data.Views;
using LOMs.Infrastructure.Data.Views.ClientFiles;
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

            migrationBuilder.Sql(ClientFilesWithFileNumberView.Sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("IF OBJECT_ID('[Case].[vw_ClientFilesWithFileNumber]', 'V') IS NOT NULL DROP VIEW [Case].[vw_ClientFilesWithFileNumber];");
        }
    }
}
