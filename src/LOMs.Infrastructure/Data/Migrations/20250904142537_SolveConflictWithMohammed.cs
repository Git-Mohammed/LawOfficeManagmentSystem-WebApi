using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOMs.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SolveConflictWithMohammed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedOfficer",
                table: "Cases");

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedOfficerId",
                table: "Cases",
                type: "uniqueidentifier",
                maxLength: 100,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "اسم الموظف المسؤول عن متابعة القضية");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "Cases",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Cases_EmployeeId",
                table: "Cases",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Employees_EmployeeId",
                table: "Cases",
                column: "EmployeeId",
                principalSchema: "UserManagement",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Employees_EmployeeId",
                table: "Cases");

            migrationBuilder.DropIndex(
                name: "IX_Cases_EmployeeId",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "AssignedOfficerId",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Cases");

            migrationBuilder.AddColumn<string>(
                name: "AssignedOfficer",
                table: "Cases",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                comment: "اسم الموظف المسؤول عن متابعة القضية");
        }
    }
}
