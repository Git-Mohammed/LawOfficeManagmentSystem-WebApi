using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOMs.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCourtTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Employees_AssignedEmployeeId",
                table: "Cases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contracts",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "CourtType",
                table: "Cases");

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "ContractNumber",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<short>(
                name: "CourtTypeCode",
                table: "Contracts",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "HijriYear",
                table: "Contracts",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "OrderNumber",
                table: "Contracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<short>(
                name: "CourtTypeCode",
                table: "ClientFiles",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "HijriYear",
                table: "ClientFiles",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "OrderNumber",
                table: "ClientFiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "EstimatedReviewDate",
                table: "Cases",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CourtTypeId",
                table: "Cases",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contracts",
                table: "Contracts",
                column: "Id")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateTable(
                name: "CourtTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<short>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourtTypes", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cases_CourtTypeId",
                table: "Cases",
                column: "CourtTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_CourtTypes_CourtTypeId",
                table: "Cases",
                column: "CourtTypeId",
                principalTable: "CourtTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Employees_AssignedEmployeeId",
                table: "Cases",
                column: "AssignedEmployeeId",
                principalSchema: "UserManagement",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_CourtTypes_CourtTypeId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Employees_AssignedEmployeeId",
                table: "Cases");

            migrationBuilder.DropTable(
                name: "CourtTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contracts",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Cases_CourtTypeId",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "CourtTypeCode",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "HijriYear",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "CourtTypeCode",
                table: "ClientFiles");

            migrationBuilder.DropColumn(
                name: "HijriYear",
                table: "ClientFiles");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "ClientFiles");

            migrationBuilder.DropColumn(
                name: "CourtTypeId",
                table: "Cases");

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "Contracts",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ContractNumber",
                table: "Contracts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "EstimatedReviewDate",
                table: "Cases",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<int>(
                name: "CourtType",
                table: "Cases",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "نوع المحكمة: 100 = عامة، 200 = جزئية، 300 = عمالية، 400 = أحوال شخصية، 600 = إدارية، 700 = لجان شبه قضائية، 800 = أخرى");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contracts",
                table: "Contracts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Employees_AssignedEmployeeId",
                table: "Cases",
                column: "AssignedEmployeeId",
                principalSchema: "UserManagement",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
