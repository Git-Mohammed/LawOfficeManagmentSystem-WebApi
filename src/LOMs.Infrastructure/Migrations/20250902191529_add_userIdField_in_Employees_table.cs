using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOMs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_userIdField_in_Employees_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "UserManagement",
                table: "Employees",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                schema: "UserManagement",
                table: "Employees",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_UserId",
                schema: "UserManagement",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "UserManagement",
                table: "Employees");
        }
    }
}
