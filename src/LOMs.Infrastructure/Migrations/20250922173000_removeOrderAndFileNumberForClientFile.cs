using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOMs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeOrderAndFileNumberForClientFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClientFiles_FileNumber",
                schema: "Case",
                table: "ClientFiles");

            migrationBuilder.DropColumn(
                name: "FileNumber",
                schema: "Case",
                table: "ClientFiles");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                schema: "Case",
                table: "ClientFiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileNumber",
                schema: "Case",
                table: "ClientFiles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OrderNumber",
                schema: "Case",
                table: "ClientFiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ClientFiles_FileNumber",
                schema: "Case",
                table: "ClientFiles",
                column: "FileNumber",
                unique: true);
        }
    }
}
