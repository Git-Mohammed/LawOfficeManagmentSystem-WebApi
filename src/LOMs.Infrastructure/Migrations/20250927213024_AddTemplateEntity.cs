using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOMs.Infrastructure.Migrations
{
    public partial class AddTemplateEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // تأكد من أن الـ Schema موجود
            migrationBuilder.EnsureSchema(
                name: "Templates");

            // إنشاء جدول Templates فقط
            migrationBuilder.CreateTable(
                name: "Templates",
                schema: "Templates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Filename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Filepath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // حذف جدول Templates فقط
            migrationBuilder.DropTable(
                name: "Templates",
                schema: "Templates");
        }
    }
}
