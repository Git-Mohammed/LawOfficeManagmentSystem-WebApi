using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOMs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addContractFilesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                schema: "Finance",
                table: "Contracts");

            migrationBuilder.CreateTable(
                name: "ContractFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractFiles", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_ContractFiles_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalSchema: "Finance",
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractFiles_ContractId",
                table: "ContractFiles",
                column: "ContractId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractFiles");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                schema: "Finance",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
