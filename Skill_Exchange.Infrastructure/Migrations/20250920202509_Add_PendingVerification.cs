using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Skill_Exchange.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_PendingVerification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PendingVerifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VerificationCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expiry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingVerifications", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ConcurrencyStamp", "LastActiveAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ccd49db7-14c3-4267-9a9a-7f070e4d0647", new DateTime(2025, 9, 20, 20, 25, 7, 23, DateTimeKind.Utc).AddTicks(6229), "AQAAAAIAAYagAAAAEAWfDbBg+BSsCjUiwNbnoENfgrpVFMc7g/Gccs5s+dgMU0Eymqkl7vQgxYSPdscLNA==", "1d13dbde-406d-43da-bb90-9c3fd8cdfd1a" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ConcurrencyStamp", "LastActiveAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f8c4efc6-94a7-4706-b97d-a5e6a2adcf36", new DateTime(2025, 9, 20, 20, 25, 7, 273, DateTimeKind.Utc).AddTicks(848), "AQAAAAIAAYagAAAAEEDFpSQm/cVqFuB0c6+jEuhUTCzeEdqDogURe5yGCW1lKT3GiWZHaDns465XlEBE0g==", "78e506f1-d22c-420f-8111-eee2d5fed753" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PendingVerifications");

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
        }
    }
}
