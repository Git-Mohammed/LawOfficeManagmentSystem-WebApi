using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOMs.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class renameTotalAmountToBaseAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Contracts",
                newName: "BaseAmount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BaseAmount",
                table: "Contracts",
                newName: "TotalAmount");
        }
    }
}
