using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOMs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class renameIsUsingTempPasswordColumnInUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsUsingTempPassowrd",
                schema: "Identity",
                table: "Users",
                newName: "IsUsingTempPassword");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsUsingTempPassword",
                schema: "Identity",
                table: "Users",
                newName: "IsUsingTempPassowrd");
        }
    }
}
