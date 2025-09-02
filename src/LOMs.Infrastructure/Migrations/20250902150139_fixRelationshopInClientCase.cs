using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOMs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixRelationshopInClientCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientCases_ClientFiles_ClientId",
                table: "ClientCases");

            migrationBuilder.CreateIndex(
                name: "IX_ClientCases_ClientFileId",
                table: "ClientCases",
                column: "ClientFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientCases_ClientFiles_ClientFileId",
                table: "ClientCases",
                column: "ClientFileId",
                principalTable: "ClientFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientCases_ClientFiles_ClientFileId",
                table: "ClientCases");

            migrationBuilder.DropIndex(
                name: "IX_ClientCases_ClientFileId",
                table: "ClientCases");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientCases_ClientFiles_ClientId",
                table: "ClientCases",
                column: "ClientId",
                principalTable: "ClientFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
