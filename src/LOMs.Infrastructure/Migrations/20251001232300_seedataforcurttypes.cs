using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LOMs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedataforcurttypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "CaseManagment",
                table: "CourtTypes",
                columns: new[] { "Id", "Code", "CreatedAtUtc", "CreatedBy", "Description", "LastModifiedBy", "LastModifiedUtc", "Name" },
                values: new object[,]
                {
                    { new Guid("1816e3fb-6fb0-428f-cf6f-5a7b7c829e10"), (short)700, new DateTimeOffset(new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System", "اللجان شبه القضائية", "System", new DateTimeOffset(new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "اللجان شبه القضائية" },
                    { new Guid("a8b6e3fb-6fb0-4f8f-cf6f-5a6b7c8d9e10"), (short)600, new DateTimeOffset(new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System", "المحكمة الإدارية", "System", new DateTimeOffset(new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "الإدارية" },
                    { new Guid("b3a1f8b4-1e6a-4b3e-9c3d-0a9f8d8a1b11"), (short)100, new DateTimeOffset(new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System", "المحكمة العامة", "System", new DateTimeOffset(new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "العامة" },
                    { new Guid("b9c7f40c-70c1-4f9f-df7f-6b7c8d9e0f21"), (short)32000, new DateTimeOffset(new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System", "أخرى", "System", new DateTimeOffset(new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "أخرى" },
                    { new Guid("c4f2a9d7-2b7e-4a4f-8b2e-1c2d3e4f5a66"), (short)200, new DateTimeOffset(new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System", "المحكمة الجنائية", "System", new DateTimeOffset(new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "الجنائية" },
                    { new Guid("d5e3b0c8-3c8f-4c5f-9d3f-2d3e4f5a6b77"), (short)300, new DateTimeOffset(new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System", "المحكمة العمالية", "System", new DateTimeOffset(new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "العمالية" },
                    { new Guid("e6f4c1d9-4d9f-4d6f-ae4f-3e4f5a6b7c88"), (short)400, new DateTimeOffset(new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System", "محكمة الأحوال الشخصية", "System", new DateTimeOffset(new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "الأحوال الشخصية" },
                    { new Guid("f7a5d2ea-5eaf-4e7f-bf5f-4f5a6b7c8d99"), (short)500, new DateTimeOffset(new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System", "المحكمة التجارية", "System", new DateTimeOffset(new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "التجارية" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "CaseManagment",
                table: "CourtTypes",
                keyColumn: "Id",
                keyValue: new Guid("1816e3fb-6fb0-428f-cf6f-5a7b7c829e10"));

            migrationBuilder.DeleteData(
                schema: "CaseManagment",
                table: "CourtTypes",
                keyColumn: "Id",
                keyValue: new Guid("a8b6e3fb-6fb0-4f8f-cf6f-5a6b7c8d9e10"));

            migrationBuilder.DeleteData(
                schema: "CaseManagment",
                table: "CourtTypes",
                keyColumn: "Id",
                keyValue: new Guid("b3a1f8b4-1e6a-4b3e-9c3d-0a9f8d8a1b11"));

            migrationBuilder.DeleteData(
                schema: "CaseManagment",
                table: "CourtTypes",
                keyColumn: "Id",
                keyValue: new Guid("b9c7f40c-70c1-4f9f-df7f-6b7c8d9e0f21"));

            migrationBuilder.DeleteData(
                schema: "CaseManagment",
                table: "CourtTypes",
                keyColumn: "Id",
                keyValue: new Guid("c4f2a9d7-2b7e-4a4f-8b2e-1c2d3e4f5a66"));

            migrationBuilder.DeleteData(
                schema: "CaseManagment",
                table: "CourtTypes",
                keyColumn: "Id",
                keyValue: new Guid("d5e3b0c8-3c8f-4c5f-9d3f-2d3e4f5a6b77"));

            migrationBuilder.DeleteData(
                schema: "CaseManagment",
                table: "CourtTypes",
                keyColumn: "Id",
                keyValue: new Guid("e6f4c1d9-4d9f-4d6f-ae4f-3e4f5a6b7c88"));

            migrationBuilder.DeleteData(
                schema: "CaseManagment",
                table: "CourtTypes",
                keyColumn: "Id",
                keyValue: new Guid("f7a5d2ea-5eaf-4e7f-bf5f-4f5a6b7c8d99"));
        }
    }
}
