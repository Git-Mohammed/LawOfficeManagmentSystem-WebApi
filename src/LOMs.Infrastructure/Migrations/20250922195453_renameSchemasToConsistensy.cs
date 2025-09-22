using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOMs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class renameSchemasToConsistensy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CaseManagment");

            migrationBuilder.EnsureSchema(
                name: "UserManagment");

            migrationBuilder.EnsureSchema(
                name: "FileManagment");

            migrationBuilder.EnsureSchema(
                name: "FinanceManagment");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                schema: "User",
                newName: "RefreshTokens",
                newSchema: "UserManagment");

            migrationBuilder.RenameTable(
                name: "POAs",
                schema: "Case",
                newName: "POAs",
                newSchema: "CaseManagment");

            migrationBuilder.RenameTable(
                name: "POAFiles",
                schema: "FileManagement",
                newName: "POAFiles",
                newSchema: "FileManagment");

            migrationBuilder.RenameTable(
                name: "People",
                schema: "User",
                newName: "People",
                newSchema: "UserManagment");

            migrationBuilder.RenameTable(
                name: "Employees",
                schema: "User",
                newName: "Employees",
                newSchema: "UserManagment");

            migrationBuilder.RenameTable(
                name: "CourtTypes",
                schema: "Case",
                newName: "CourtTypes",
                newSchema: "CaseManagment");

            migrationBuilder.RenameTable(
                name: "CourtSessions",
                schema: "Case",
                newName: "CourtSessions",
                newSchema: "CaseManagment");

            migrationBuilder.RenameTable(
                name: "Contracts",
                schema: "Finance",
                newName: "Contracts",
                newSchema: "FinanceManagment");

            migrationBuilder.RenameTable(
                name: "ContractFiles",
                schema: "FileManagement",
                newName: "ContractFiles",
                newSchema: "FileManagment");

            migrationBuilder.RenameTable(
                name: "Clients",
                schema: "User",
                newName: "Clients",
                newSchema: "UserManagment");

            migrationBuilder.RenameTable(
                name: "ClientFiles",
                schema: "Case",
                newName: "ClientFiles",
                newSchema: "CaseManagment");

            migrationBuilder.RenameTable(
                name: "ClientCases",
                schema: "Case",
                newName: "ClientCases",
                newSchema: "CaseManagment");

            migrationBuilder.RenameTable(
                name: "Cases",
                schema: "Case",
                newName: "Cases",
                newSchema: "CaseManagment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Case");

            migrationBuilder.EnsureSchema(
                name: "User");

            migrationBuilder.EnsureSchema(
                name: "FileManagement");

            migrationBuilder.EnsureSchema(
                name: "Finance");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                schema: "UserManagment",
                newName: "RefreshTokens",
                newSchema: "User");

            migrationBuilder.RenameTable(
                name: "POAs",
                schema: "CaseManagment",
                newName: "POAs",
                newSchema: "Case");

            migrationBuilder.RenameTable(
                name: "POAFiles",
                schema: "FileManagment",
                newName: "POAFiles",
                newSchema: "FileManagement");

            migrationBuilder.RenameTable(
                name: "People",
                schema: "UserManagment",
                newName: "People",
                newSchema: "User");

            migrationBuilder.RenameTable(
                name: "Employees",
                schema: "UserManagment",
                newName: "Employees",
                newSchema: "User");

            migrationBuilder.RenameTable(
                name: "CourtTypes",
                schema: "CaseManagment",
                newName: "CourtTypes",
                newSchema: "Case");

            migrationBuilder.RenameTable(
                name: "CourtSessions",
                schema: "CaseManagment",
                newName: "CourtSessions",
                newSchema: "Case");

            migrationBuilder.RenameTable(
                name: "Contracts",
                schema: "FinanceManagment",
                newName: "Contracts",
                newSchema: "Finance");

            migrationBuilder.RenameTable(
                name: "ContractFiles",
                schema: "FileManagment",
                newName: "ContractFiles",
                newSchema: "FileManagement");

            migrationBuilder.RenameTable(
                name: "Clients",
                schema: "UserManagment",
                newName: "Clients",
                newSchema: "User");

            migrationBuilder.RenameTable(
                name: "ClientFiles",
                schema: "CaseManagment",
                newName: "ClientFiles",
                newSchema: "Case");

            migrationBuilder.RenameTable(
                name: "ClientCases",
                schema: "CaseManagment",
                newName: "ClientCases",
                newSchema: "Case");

            migrationBuilder.RenameTable(
                name: "Cases",
                schema: "CaseManagment",
                newName: "Cases",
                newSchema: "Case");
        }
    }
}
