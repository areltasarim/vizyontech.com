using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "IskontoOrani",
                table: "AspNetUsers",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "c911ebb3-9df4-4d8a-ace1-7983c4cea28c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "89548ba6-d69b-401d-a1b4-bc2e0bb8f6aa");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "f8d809f7-6185-4130-8b46-f56559cdba28");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a8112551-2d07-45df-a25e-d9fa5fe9a32c", "AQAAAAIAAYagAAAAEHn2AfYNU9xHjw/5WSZruLpRga1YOnMaokYUO9AGmgtym/IAoiJgFzeaI303m4EmFw==", "4bc5ae7b-4826-4067-a179-bd89c3f75f8a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "IskontoOrani",
                table: "AspNetUsers",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "0015eba5-e737-44c0-95f6-ac4f88d2d9e8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "76ffb813-f67e-423b-9539-c21efece4075");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "e1048665-f0ed-4237-afe3-3ca7b79629b1");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "36f5386a-a98b-4b36-8ef6-a81f0a802d4f", "AQAAAAIAAYagAAAAEMrceIxLYAfRuqwND5ZHOWDSJQRzgjtLKMIjU5hYV+ZxYCTS4WJARf/j2xxxe3rm4w==", "a711205b-42c8-4a3a-a67f-c95eba76b490" });
        }
    }
}
