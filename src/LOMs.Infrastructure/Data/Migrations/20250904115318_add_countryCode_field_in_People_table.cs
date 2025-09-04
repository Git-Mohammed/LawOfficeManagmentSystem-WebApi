using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOMs.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_countryCode_field_in_People_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "People",
                newName: "People",
                newSchema: "UserManagement");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                schema: "UserManagement",
                table: "People",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                schema: "UserManagement",
                table: "People",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_People_NationalId",
                schema: "UserManagement",
                table: "People",
                column: "NationalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_People_NationalId",
                schema: "UserManagement",
                table: "People");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                schema: "UserManagement",
                table: "People");

            migrationBuilder.RenameTable(
                name: "People",
                schema: "UserManagement",
                newName: "People");

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
    }
}
