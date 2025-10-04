using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOMs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOrderNumberAndContractNumberFromContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractNumber",
                schema: "FinanceManagment",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                schema: "FinanceManagment",
                table: "Contracts");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                schema: "FinanceManagment",
                table: "Contracts",
                type: "int",
                nullable: false,
                comment: "Contract type: 1 = Fixed Term, 2 = Open Term",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "نوع العقد: 1 = محدد، 2 = غير محدد");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Type",
                schema: "FinanceManagment",
                table: "Contracts",
                type: "int",
                nullable: false,
                comment: "نوع العقد: 1 = محدد، 2 = غير محدد",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Contract type: 1 = Fixed Term, 2 = Open Term");

            migrationBuilder.AddColumn<string>(
                name: "ContractNumber",
                schema: "FinanceManagment",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OrderNumber",
                schema: "FinanceManagment",
                table: "Contracts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
