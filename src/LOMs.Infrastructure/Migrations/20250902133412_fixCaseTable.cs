using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOMs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixCaseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_ClientFiles_ClientFileId",
                table: "Cases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientCases",
                table: "ClientCases");

            migrationBuilder.DropIndex(
                name: "IX_Cases_ClientFileId",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "ClientFileId",
                table: "Cases");

            migrationBuilder.AddColumn<Guid>(
                name: "ClientFileId",
                table: "ClientCases",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientCases",
                table: "ClientCases",
                columns: new[] { "CaseId", "ClientId", "ClientFileId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ClientCases_ClientFiles_ClientId",
                table: "ClientCases",
                column: "ClientId",
                principalTable: "ClientFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientCases_ClientFiles_ClientId",
                table: "ClientCases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientCases",
                table: "ClientCases");

            migrationBuilder.DropColumn(
                name: "ClientFileId",
                table: "ClientCases");

            migrationBuilder.AddColumn<Guid>(
                name: "ClientFileId",
                table: "Cases",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientCases",
                table: "ClientCases",
                columns: new[] { "CaseId", "ClientId" });

            migrationBuilder.CreateIndex(
                name: "IX_Cases_ClientFileId",
                table: "Cases",
                column: "ClientFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_ClientFiles_ClientFileId",
                table: "Cases",
                column: "ClientFileId",
                principalTable: "ClientFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
