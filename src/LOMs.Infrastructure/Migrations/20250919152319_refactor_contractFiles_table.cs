using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOMs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class refactor_contractFiles_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "FileManagement");

            migrationBuilder.RenameTable(
                name: "ContractFiles",
                newName: "ContractFiles",
                newSchema: "FileManagement");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                schema: "FileManagement",
                table: "ContractFiles",
                newName: "Filepath");

            migrationBuilder.RenameColumn(
                name: "FileName",
                schema: "FileManagement",
                table: "ContractFiles",
                newName: "Filename");

            migrationBuilder.AlterColumn<string>(
                name: "Filepath",
                schema: "FileManagement",
                table: "ContractFiles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Filename",
                schema: "FileManagement",
                table: "ContractFiles",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                schema: "FileManagement",
                table: "ContractFiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                schema: "FileManagement",
                table: "ContractFiles",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                schema: "FileManagement",
                table: "ContractFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Size",
                schema: "FileManagement",
                table: "ContractFiles",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                schema: "FileManagement",
                table: "ContractFiles");

            migrationBuilder.DropColumn(
                name: "Extension",
                schema: "FileManagement",
                table: "ContractFiles");

            migrationBuilder.DropColumn(
                name: "FileType",
                schema: "FileManagement",
                table: "ContractFiles");

            migrationBuilder.DropColumn(
                name: "Size",
                schema: "FileManagement",
                table: "ContractFiles");

            migrationBuilder.RenameTable(
                name: "ContractFiles",
                schema: "FileManagement",
                newName: "ContractFiles");

            migrationBuilder.RenameColumn(
                name: "Filepath",
                table: "ContractFiles",
                newName: "FilePath");

            migrationBuilder.RenameColumn(
                name: "Filename",
                table: "ContractFiles",
                newName: "FileName");

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "ContractFiles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "ContractFiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);
        }
    }
}
