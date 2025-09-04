using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOMs.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameAssignUserIdToAssignEmployeeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AssignedOfficerId",
                table: "Cases",
                newName: "AssignedEmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AssignedEmployeeId",
                table: "Cases",
                newName: "AssignedOfficerId");
        }
    }
}
