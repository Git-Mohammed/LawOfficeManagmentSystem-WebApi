using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOMs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Cases_CaseId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_CaseId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CaseId",
                table: "Clients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CaseId",
                table: "Clients",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CaseId",
                table: "Clients",
                column: "CaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Cases_CaseId",
                table: "Clients",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id");
        }
    }
}
