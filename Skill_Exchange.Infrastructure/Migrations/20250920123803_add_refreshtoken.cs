using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Skill_Exchange.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_refreshtoken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ConcurrencyStamp", "LastActiveAt", "PasswordHash", "RefreshToken", "RefreshTokenExpiryTime", "SecurityStamp" },
                values: new object[] { "475a08e3-e7f3-4320-905b-2e03393a9b95", new DateTime(2025, 9, 20, 12, 38, 1, 512, DateTimeKind.Utc).AddTicks(8716), "AQAAAAIAAYagAAAAEC1okznvWmiFQ5FhVOeBoaXJ7H5ga9pTWpVY/BcsgTIj2JwhB9zZ6G2ufIj/YkokZA==", null, null, "2e08c3b6-f40e-4cfd-960f-5e4d985663ab" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ConcurrencyStamp", "LastActiveAt", "PasswordHash", "RefreshToken", "RefreshTokenExpiryTime", "SecurityStamp" },
                values: new object[] { "ae6ff25b-966a-4f86-b7a3-f678e412eb13", new DateTime(2025, 9, 20, 12, 38, 1, 632, DateTimeKind.Utc).AddTicks(6760), "AQAAAAIAAYagAAAAECTmvuzOOS5BvgkxejEdn1m5LMZk0lHI6s3AqKue5BzGP7O/uhmqX2SrHspINMlfQA==", null, null, "da61298c-2375-432b-b1fd-552c6c3daed3" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ConcurrencyStamp", "LastActiveAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7cb3b1c9-50b4-4e2e-9951-5dc1e79caa6d", new DateTime(2025, 9, 9, 22, 28, 44, 664, DateTimeKind.Utc).AddTicks(5369), "AQAAAAIAAYagAAAAEGEo1aS/8P2B7Yvhool5fsBbtOx5/ljBBZqDxEIHpElChiB/MqyPOLR0amf85rqP6Q==", "0451a3f8-6273-4dc4-912b-21529a6f2526" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ConcurrencyStamp", "LastActiveAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8e3283c4-9750-4127-aeaa-1603df90e6c1", new DateTime(2025, 9, 9, 22, 28, 44, 781, DateTimeKind.Utc).AddTicks(6065), "AQAAAAIAAYagAAAAEEKRBOUfKmeYdXc5FooggUtXH+2lI4lNiVS5ttj2yrZQjtZxuxmWH9NgVcqYHiYDAg==", "e3a61a22-1b6a-4982-96bc-33ee935e2c3a" });
        }
    }
}
