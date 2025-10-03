using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOMs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefineColumnDataTypesAndLengths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_Token",
                schema: "UserManagment",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_People_NationalId",
                schema: "UserManagment",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Email",
                schema: "UserManagment",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_UserId",
                schema: "UserManagment",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Attachments",
                schema: "CaseManagment",
                table: "Attachments");

            migrationBuilder.RenameTable(
                name: "Judgments",
                schema: "CaseManagement",
                newName: "Judgments",
                newSchema: "CaseManagment");

            migrationBuilder.RenameTable(
                name: "JudgmentFiles",
                schema: "CaseManagement",
                newName: "JudgmentFiles",
                newSchema: "CaseManagment");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_PersonId",
                schema: "UserManagment",
                table: "Employees",
                newName: "UX_Employees_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Clients_PersonId",
                schema: "UserManagment",
                table: "Clients",
                newName: "UX_Clients_PersonId");

            migrationBuilder.AlterColumn<string>(
                name: "TemplateName",
                schema: "Templates",
                table: "Templates",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Filepath",
                schema: "Templates",
                table: "Templates",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Filename",
                schema: "Templates",
                table: "Templates",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FileType",
                schema: "Templates",
                table: "Templates",
                type: "varchar(30)",
                unicode: false,
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                schema: "Templates",
                table: "Templates",
                type: "varchar(8)",
                unicode: false,
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                schema: "Templates",
                table: "Templates",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                schema: "UserManagment",
                table: "RefreshTokens",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "POANumber",
                schema: "CaseManagment",
                table: "POAs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldComment: "Official POA number as stated in the document");

            migrationBuilder.AlterColumn<string>(
                name: "IssuingAuthority",
                schema: "CaseManagment",
                table: "POAs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldComment: "The authority that issued the POA, such as a court or notary");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "IssueDate",
                schema: "CaseManagment",
                table: "POAs",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldComment: "Date the POA was issued");

            migrationBuilder.AlterColumn<string>(
                name: "Filepath",
                schema: "FileManagment",
                table: "POAFiles",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Filename",
                schema: "FileManagment",
                table: "POAFiles",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "FileType",
                schema: "FileManagment",
                table: "POAFiles",
                type: "varchar(30)",
                unicode: false,
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                schema: "FileManagment",
                table: "POAFiles",
                type: "varchar(8)",
                unicode: false,
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                schema: "FileManagment",
                table: "POAFiles",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                schema: "UserManagment",
                table: "Phones",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "UserManagment",
                table: "Phones",
                type: "varchar(10)",
                unicode: false,
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<string>(
                name: "NationalId",
                schema: "UserManagment",
                table: "People",
                type: "varchar(11)",
                unicode: false,
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldMaxLength: 11);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                schema: "UserManagment",
                table: "People",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "CountryCode",
                schema: "UserManagment",
                table: "People",
                type: "varchar(2)",
                unicode: false,
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                schema: "UserManagment",
                table: "Employees",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "UserManagment",
                table: "Employees",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "CaseManagment",
                table: "CourtTypes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "FinanceManagment",
                table: "Contracts",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Contract type: 1 = Fixed Term, 2 = Open Term");

            migrationBuilder.AlterColumn<string>(
                name: "Filepath",
                schema: "FileManagment",
                table: "ContractFiles",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Filename",
                schema: "FileManagment",
                table: "ContractFiles",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "FileType",
                schema: "FileManagment",
                table: "ContractFiles",
                type: "varchar(30)",
                unicode: false,
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                schema: "FileManagment",
                table: "ContractFiles",
                type: "varchar(8)",
                unicode: false,
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                schema: "FileManagment",
                table: "ContractFiles",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "CaseManagment",
                table: "Cases",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Current status of the case: 0 = Draft, 1 = Pending, 2 = In Progress, 3 = Completed, 4 = Cancelled");

            migrationBuilder.AlterColumn<string>(
                name: "PartyRole",
                schema: "CaseManagment",
                table: "Cases",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: false,
                comment: "Client's role: Plaintiff or Defendant",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Client's role in the case: 1 = Plaintiff, 2 = Defendant");

            migrationBuilder.AlterColumn<string>(
                name: "ClientRequests",
                schema: "CaseManagment",
                table: "Cases",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CaseNumber",
                schema: "CaseManagment",
                table: "Cases",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AssignedEmployeeId",
                schema: "CaseManagment",
                table: "Cases",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldMaxLength: 100,
                oldComment: "The ID of the employee assigned to follow up on the case");

            migrationBuilder.AlterColumn<string>(
                name: "Filepath",
                schema: "CaseManagment",
                table: "Attachments",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Filename",
                schema: "CaseManagment",
                table: "Attachments",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FileType",
                schema: "CaseManagment",
                table: "Attachments",
                type: "varchar(30)",
                unicode: false,
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                schema: "CaseManagment",
                table: "Attachments",
                type: "varchar(8)",
                unicode: false,
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                schema: "CaseManagment",
                table: "Attachments",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AttachmentName",
                schema: "CaseManagment",
                table: "Attachments",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "CaseManagment",
                table: "Judgments",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SubType",
                schema: "CaseManagment",
                table: "Judgments",
                type: "varchar(30)",
                unicode: false,
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                schema: "CaseManagment",
                table: "Judgments",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Filepath",
                schema: "CaseManagment",
                table: "JudgmentFiles",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Filename",
                schema: "CaseManagment",
                table: "JudgmentFiles",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "FileType",
                schema: "CaseManagment",
                table: "JudgmentFiles",
                type: "varchar(30)",
                unicode: false,
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                schema: "CaseManagment",
                table: "JudgmentFiles",
                type: "varchar(8)",
                unicode: false,
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                schema: "CaseManagment",
                table: "JudgmentFiles",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attachments",
                schema: "CaseManagment",
                table: "Attachments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_TemplateName",
                schema: "Templates",
                table: "Templates",
                column: "TemplateName");

            migrationBuilder.CreateIndex(
                name: "UX_RefreshTokens_Token",
                schema: "UserManagment",
                table: "RefreshTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_POAFiles_POAId_FileType",
                schema: "FileManagment",
                table: "POAFiles",
                columns: new[] { "POAId", "FileType" });

            migrationBuilder.CreateIndex(
                name: "IX_People_CountryCode",
                schema: "UserManagment",
                table: "People",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "UX_People_NationalId",
                schema: "UserManagment",
                table: "People",
                column: "NationalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Employees_Email",
                schema: "UserManagment",
                table: "Employees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Employees_UserId",
                schema: "UserManagment",
                table: "Employees",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourtTypes_Name",
                schema: "CaseManagment",
                table: "CourtTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "UX_CourtTypes_Code",
                schema: "CaseManagment",
                table: "CourtTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CourtTypeCode",
                schema: "FinanceManagment",
                table: "Contracts",
                column: "CourtTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_HijriYear",
                schema: "FinanceManagment",
                table: "Contracts",
                column: "HijriYear");

            migrationBuilder.CreateIndex(
                name: "IX_ContractFiles_ContractId_FileType",
                schema: "FileManagment",
                table: "ContractFiles",
                columns: new[] { "ContractId", "FileType" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientFiles_CourtTypeCode",
                schema: "CaseManagment",
                table: "ClientFiles",
                column: "CourtTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_PartyRole",
                schema: "CaseManagment",
                table: "Cases",
                column: "PartyRole");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_Status",
                schema: "CaseManagment",
                table: "Cases",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_JudgmentFiles_JudgmentId_FileType",
                schema: "CaseManagment",
                table: "JudgmentFiles",
                columns: new[] { "JudgmentId", "FileType" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Templates_TemplateName",
                schema: "Templates",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "UX_RefreshTokens_Token",
                schema: "UserManagment",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_POAFiles_POAId_FileType",
                schema: "FileManagment",
                table: "POAFiles");

            migrationBuilder.DropIndex(
                name: "IX_People_CountryCode",
                schema: "UserManagment",
                table: "People");

            migrationBuilder.DropIndex(
                name: "UX_People_NationalId",
                schema: "UserManagment",
                table: "People");

            migrationBuilder.DropIndex(
                name: "UX_Employees_Email",
                schema: "UserManagment",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "UX_Employees_UserId",
                schema: "UserManagment",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_CourtTypes_Name",
                schema: "CaseManagment",
                table: "CourtTypes");

            migrationBuilder.DropIndex(
                name: "UX_CourtTypes_Code",
                schema: "CaseManagment",
                table: "CourtTypes");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_CourtTypeCode",
                schema: "FinanceManagment",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_HijriYear",
                schema: "FinanceManagment",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_ContractFiles_ContractId_FileType",
                schema: "FileManagment",
                table: "ContractFiles");

            migrationBuilder.DropIndex(
                name: "IX_ClientFiles_CourtTypeCode",
                schema: "CaseManagment",
                table: "ClientFiles");

            migrationBuilder.DropIndex(
                name: "IX_Cases_PartyRole",
                schema: "CaseManagment",
                table: "Cases");

            migrationBuilder.DropIndex(
                name: "IX_Cases_Status",
                schema: "CaseManagment",
                table: "Cases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Attachments",
                schema: "CaseManagment",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_JudgmentFiles_JudgmentId_FileType",
                schema: "CaseManagment",
                table: "JudgmentFiles");

            migrationBuilder.EnsureSchema(
                name: "CaseManagement");

            migrationBuilder.RenameTable(
                name: "Judgments",
                schema: "CaseManagment",
                newName: "Judgments",
                newSchema: "CaseManagement");

            migrationBuilder.RenameTable(
                name: "JudgmentFiles",
                schema: "CaseManagment",
                newName: "JudgmentFiles",
                newSchema: "CaseManagement");

            migrationBuilder.RenameIndex(
                name: "UX_Employees_PersonId",
                schema: "UserManagment",
                table: "Employees",
                newName: "IX_Employees_PersonId");

            migrationBuilder.RenameIndex(
                name: "UX_Clients_PersonId",
                schema: "UserManagment",
                table: "Clients",
                newName: "IX_Clients_PersonId");

            migrationBuilder.AlterColumn<string>(
                name: "TemplateName",
                schema: "Templates",
                table: "Templates",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldUnicode: false,
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Filepath",
                schema: "Templates",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldUnicode: false,
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Filename",
                schema: "Templates",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldUnicode: false,
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<int>(
                name: "FileType",
                schema: "Templates",
                table: "Templates",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldUnicode: false,
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                schema: "Templates",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(8)",
                oldUnicode: false,
                oldMaxLength: 8);

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                schema: "Templates",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                schema: "UserManagment",
                table: "RefreshTokens",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "POANumber",
                schema: "CaseManagment",
                table: "POAs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                comment: "Official POA number as stated in the document",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "IssuingAuthority",
                schema: "CaseManagment",
                table: "POAs",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                comment: "The authority that issued the POA, such as a court or notary",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "IssueDate",
                schema: "CaseManagment",
                table: "POAs",
                type: "date",
                nullable: false,
                comment: "Date the POA was issued",
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Filepath",
                schema: "FileManagment",
                table: "POAFiles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldUnicode: false,
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Filename",
                schema: "FileManagment",
                table: "POAFiles",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldUnicode: false,
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "FileType",
                schema: "FileManagment",
                table: "POAFiles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldUnicode: false,
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                schema: "FileManagment",
                table: "POAFiles",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(8)",
                oldUnicode: false,
                oldMaxLength: 8);

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                schema: "FileManagment",
                table: "POAFiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                schema: "UserManagment",
                table: "Phones",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldUnicode: false,
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "UserManagment",
                table: "Phones",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldUnicode: false,
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "NationalId",
                schema: "UserManagment",
                table: "People",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(11)",
                oldUnicode: false,
                oldMaxLength: 11);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                schema: "UserManagment",
                table: "People",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "CountryCode",
                schema: "UserManagment",
                table: "People",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(2)",
                oldUnicode: false,
                oldMaxLength: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                schema: "UserManagment",
                table: "Employees",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "UserManagment",
                table: "Employees",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "CaseManagment",
                table: "CourtTypes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                schema: "FinanceManagment",
                table: "Contracts",
                type: "int",
                nullable: false,
                comment: "Contract type: 1 = Fixed Term, 2 = Open Term",
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldUnicode: false,
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Filepath",
                schema: "FileManagment",
                table: "ContractFiles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldUnicode: false,
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Filename",
                schema: "FileManagment",
                table: "ContractFiles",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldUnicode: false,
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "FileType",
                schema: "FileManagment",
                table: "ContractFiles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldUnicode: false,
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                schema: "FileManagment",
                table: "ContractFiles",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(8)",
                oldUnicode: false,
                oldMaxLength: 8);

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                schema: "FileManagment",
                table: "ContractFiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                schema: "CaseManagment",
                table: "Cases",
                type: "int",
                nullable: false,
                comment: "Current status of the case: 0 = Draft, 1 = Pending, 2 = In Progress, 3 = Completed, 4 = Cancelled",
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldUnicode: false,
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "PartyRole",
                schema: "CaseManagment",
                table: "Cases",
                type: "int",
                nullable: false,
                comment: "Client's role in the case: 1 = Plaintiff, 2 = Defendant",
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldUnicode: false,
                oldMaxLength: 20,
                oldComment: "Client's role: Plaintiff or Defendant");

            migrationBuilder.AlterColumn<string>(
                name: "ClientRequests",
                schema: "CaseManagment",
                table: "Cases",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CaseNumber",
                schema: "CaseManagment",
                table: "Cases",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AssignedEmployeeId",
                schema: "CaseManagment",
                table: "Cases",
                type: "uniqueidentifier",
                maxLength: 100,
                nullable: false,
                comment: "The ID of the employee assigned to follow up on the case",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Filepath",
                schema: "CaseManagment",
                table: "Attachments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldUnicode: false,
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Filename",
                schema: "CaseManagment",
                table: "Attachments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldUnicode: false,
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<int>(
                name: "FileType",
                schema: "CaseManagment",
                table: "Attachments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldUnicode: false,
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                schema: "CaseManagment",
                table: "Attachments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(8)",
                oldUnicode: false,
                oldMaxLength: 8);

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                schema: "CaseManagment",
                table: "Attachments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "AttachmentName",
                schema: "CaseManagment",
                table: "Attachments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldUnicode: false,
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "CaseManagement",
                table: "Judgments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldUnicode: false,
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "SubType",
                schema: "CaseManagement",
                table: "Judgments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldUnicode: false,
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                schema: "CaseManagement",
                table: "Judgments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldUnicode: false,
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Filepath",
                schema: "CaseManagement",
                table: "JudgmentFiles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldUnicode: false,
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Filename",
                schema: "CaseManagement",
                table: "JudgmentFiles",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldUnicode: false,
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "FileType",
                schema: "CaseManagement",
                table: "JudgmentFiles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldUnicode: false,
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                schema: "CaseManagement",
                table: "JudgmentFiles",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(8)",
                oldUnicode: false,
                oldMaxLength: 8);

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                schema: "CaseManagement",
                table: "JudgmentFiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attachments",
                schema: "CaseManagment",
                table: "Attachments",
                column: "Id")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                schema: "UserManagment",
                table: "RefreshTokens",
                column: "Token",
                unique: true,
                filter: "[Token] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_People_NationalId",
                schema: "UserManagment",
                table: "People",
                column: "NationalId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                schema: "UserManagment",
                table: "Employees",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                schema: "UserManagment",
                table: "Employees",
                column: "UserId");
        }
    }
}
