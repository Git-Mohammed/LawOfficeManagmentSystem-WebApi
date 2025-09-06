using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOMs.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class addRelationshipEmpAndCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Employees_EmployeeId",
                table: "Cases");

            migrationBuilder.DropIndex(
                name: "IX_Cases_EmployeeId",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Cases");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_AssignedEmployeeId",
                table: "Cases",
                column: "AssignedEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Employees_AssignedEmployeeId",
                table: "Cases",
                column: "AssignedEmployeeId",
                principalSchema: "UserManagement",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Employees_AssignedEmployeeId",
                table: "Cases");

            migrationBuilder.DropIndex(
                name: "IX_Cases_AssignedEmployeeId",
                table: "Cases");

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
    }
}
