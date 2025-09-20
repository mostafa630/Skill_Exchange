using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Skill_Exchange.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class make_phone_acceptNULL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ConcurrencyStamp", "LastActiveAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8458fd8a-a3e1-4c82-b242-3a279cc2536b", new DateTime(2025, 9, 20, 13, 1, 43, 678, DateTimeKind.Utc).AddTicks(3110), "AQAAAAIAAYagAAAAECUBHh/keCK0i4bS6IOFKsYb5tnKFnzX3re+SIH8lRR3SpTzD6vf322Hr7lNKAJeGg==", "3324dac1-ef66-4a17-a5cd-a1ec96f9c201" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ConcurrencyStamp", "LastActiveAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "787a3dee-23bb-4656-aa41-d3efd89fc8f5", new DateTime(2025, 9, 20, 13, 1, 43, 883, DateTimeKind.Utc).AddTicks(9402), "AQAAAAIAAYagAAAAEB1OhfcXGZrvt93OD1I+zaHrAqZwpM1emlp0Z/0nBCD+YH5L09Yy2r1HN7HoC8hZCw==", "d25b0704-5912-4986-9839-df80cdb5ff04" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ConcurrencyStamp", "LastActiveAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "475a08e3-e7f3-4320-905b-2e03393a9b95", new DateTime(2025, 9, 20, 12, 38, 1, 512, DateTimeKind.Utc).AddTicks(8716), "AQAAAAIAAYagAAAAEC1okznvWmiFQ5FhVOeBoaXJ7H5ga9pTWpVY/BcsgTIj2JwhB9zZ6G2ufIj/YkokZA==", "2e08c3b6-f40e-4cfd-960f-5e4d985663ab" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ConcurrencyStamp", "LastActiveAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ae6ff25b-966a-4f86-b7a3-f678e412eb13", new DateTime(2025, 9, 20, 12, 38, 1, 632, DateTimeKind.Utc).AddTicks(6760), "AQAAAAIAAYagAAAAECTmvuzOOS5BvgkxejEdn1m5LMZk0lHI6s3AqKue5BzGP7O/uhmqX2SrHspINMlfQA==", "da61298c-2375-432b-b1fd-552c6c3daed3" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users",
                column: "PhoneNumber",
                unique: true);
        }
    }
}
