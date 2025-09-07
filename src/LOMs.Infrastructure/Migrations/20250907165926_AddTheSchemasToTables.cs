using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOMs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTheSchemasToTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Case");

            migrationBuilder.EnsureSchema(
                name: "User");

            migrationBuilder.EnsureSchema(
                name: "Finance");

            migrationBuilder.RenameTable(
                name: "POAs",
                newName: "POAs",
                newSchema: "Case");

            migrationBuilder.RenameTable(
                name: "People",
                schema: "UserManagement",
                newName: "People",
                newSchema: "User");

            migrationBuilder.RenameTable(
                name: "Employees",
                schema: "UserManagement",
                newName: "Employees",
                newSchema: "User");

            migrationBuilder.RenameTable(
                name: "CourtTypes",
                newName: "CourtTypes",
                newSchema: "Case");

            migrationBuilder.RenameTable(
                name: "Contracts",
                newName: "Contracts",
                newSchema: "Finance");

            migrationBuilder.RenameTable(
                name: "Clients",
                newName: "Clients",
                newSchema: "User");

            migrationBuilder.RenameTable(
                name: "ClientFiles",
                newName: "ClientFiles",
                newSchema: "Case");

            migrationBuilder.RenameTable(
                name: "ClientCases",
                newName: "ClientCases",
                newSchema: "Case");

            migrationBuilder.RenameTable(
                name: "Cases",
                newName: "Cases",
                newSchema: "Case");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                schema: "Case",
                table: "Cases",
                type: "int",
                nullable: false,
                comment: "Current status of the case: 0 = Draft, 1 = Pending, 2 = In Progress, 3 = Completed, 4 = Cancelled",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "الحالة الحالية للقضية: 0 = مسودة، 1 = قيد الانتظار، 2 = قيد المعالجة، 3 = منتهية، 4 = ملغية");

            migrationBuilder.AlterColumn<int>(
                name: "PartyRole",
                schema: "Case",
                table: "Cases",
                type: "int",
                nullable: false,
                comment: "Client's role in the case: 1 = Plaintiff, 2 = Defendant",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "دور العميل في القضية: 1 = مدعي، 2 = مدعى عليه");

            migrationBuilder.AlterColumn<string>(
                name: "CaseNumber",
                schema: "Case",
                table: "Cases",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AssignedEmployeeId",
                schema: "Case",
                table: "Cases",
                type: "uniqueidentifier",
                maxLength: 100,
                nullable: false,
                comment: "The ID of the employee assigned to follow up on the case",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldMaxLength: 100,
                oldComment: "اسم الموظف المسؤول عن متابعة القضية");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "UserManagement");

            migrationBuilder.RenameTable(
                name: "POAs",
                schema: "Case",
                newName: "POAs");

            migrationBuilder.RenameTable(
                name: "People",
                schema: "User",
                newName: "People",
                newSchema: "UserManagement");

            migrationBuilder.RenameTable(
                name: "Employees",
                schema: "User",
                newName: "Employees",
                newSchema: "UserManagement");

            migrationBuilder.RenameTable(
                name: "CourtTypes",
                schema: "Case",
                newName: "CourtTypes");

            migrationBuilder.RenameTable(
                name: "Contracts",
                schema: "Finance",
                newName: "Contracts");

            migrationBuilder.RenameTable(
                name: "Clients",
                schema: "User",
                newName: "Clients");

            migrationBuilder.RenameTable(
                name: "ClientFiles",
                schema: "Case",
                newName: "ClientFiles");

            migrationBuilder.RenameTable(
                name: "ClientCases",
                schema: "Case",
                newName: "ClientCases");

            migrationBuilder.RenameTable(
                name: "Cases",
                schema: "Case",
                newName: "Cases");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Cases",
                type: "int",
                nullable: false,
                comment: "الحالة الحالية للقضية: 0 = مسودة، 1 = قيد الانتظار، 2 = قيد المعالجة، 3 = منتهية، 4 = ملغية",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Current status of the case: 0 = Draft, 1 = Pending, 2 = In Progress, 3 = Completed, 4 = Cancelled");

            migrationBuilder.AlterColumn<int>(
                name: "PartyRole",
                table: "Cases",
                type: "int",
                nullable: false,
                comment: "دور العميل في القضية: 1 = مدعي، 2 = مدعى عليه",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Client's role in the case: 1 = Plaintiff, 2 = Defendant");

            migrationBuilder.AlterColumn<string>(
                name: "CaseNumber",
                table: "Cases",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AssignedEmployeeId",
                table: "Cases",
                type: "uniqueidentifier",
                maxLength: 100,
                nullable: false,
                comment: "اسم الموظف المسؤول عن متابعة القضية",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldMaxLength: 100,
                oldComment: "The ID of the employee assigned to follow up on the case");
        }
    }
}
