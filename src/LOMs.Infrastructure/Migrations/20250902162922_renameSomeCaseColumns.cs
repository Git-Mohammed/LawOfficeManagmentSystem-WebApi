using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOMs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class renameSomeCaseColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CaseNumber",
                table: "Cases",
                newName: "Number");

            migrationBuilder.RenameColumn(
                name: "CaseNotes",
                table: "Cases",
                newName: "Subject");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "People",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Subject",
                table: "Cases",
                newName: "CaseNotes");

            migrationBuilder.RenameColumn(
                name: "Number",
                table: "Cases",
                newName: "CaseNumber");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "People",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);
        }
    }
}
