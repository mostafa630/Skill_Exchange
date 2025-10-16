using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Skill_Exchange.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NullableFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Feedback",
                table: "RatingsAndFeedbacks",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ConcurrencyStamp", "LastActiveAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5c748e7f-7ec7-4845-866c-cd2e5c74843c", new DateTime(2025, 10, 16, 15, 17, 25, 875, DateTimeKind.Utc).AddTicks(1467), "AQAAAAIAAYagAAAAEMofu3gAUliNPB+iXF9ZQLYQ6S+2JFixLDnvuy7IBeXY06XD7BnwntaoW24KW7RuqQ==", "0e98a486-d516-47ff-bf2e-77e2c754b54d" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ConcurrencyStamp", "LastActiveAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0f0f67e4-649e-4f63-a621-bbd3c70d9d6d", new DateTime(2025, 10, 16, 15, 17, 26, 60, DateTimeKind.Utc).AddTicks(1995), "AQAAAAIAAYagAAAAEBHBPlEh6fdQSKdAIQHFaE8142FVHV9hYafz8xTQBfsnjbM1Ds9JTVClrXmluqnmCA==", "37f986c4-bf07-4546-b1cf-fe9b09d3fef6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Feedback",
                table: "RatingsAndFeedbacks",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ConcurrencyStamp", "LastActiveAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "93feb779-4fdb-4b95-8699-fcc50f4978d9", new DateTime(2025, 9, 29, 17, 15, 15, 466, DateTimeKind.Utc).AddTicks(2669), "AQAAAAIAAYagAAAAEKv/3aiT7bnFPLmOaO6GIC56ZdHnRs0nFEcFf/PAMxIJErCg/Jzi7zXLZ8w2k7x2uw==", "5f2d841c-e983-4583-af86-90b1aab79030" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ConcurrencyStamp", "LastActiveAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "12c3665f-e9e4-4c40-bcd1-a3a938cc0e8a", new DateTime(2025, 9, 29, 17, 15, 15, 554, DateTimeKind.Utc).AddTicks(2579), "AQAAAAIAAYagAAAAEOeaI4mWW4w5pOdhhHPFbkT8qdoT7cMlf8vFTx1gkLmcJgw8BeA7g3PxHl/FYo5ZZw==", "ec33df1b-8368-4279-99dd-05ae19e79bb1" });
        }
    }
}
