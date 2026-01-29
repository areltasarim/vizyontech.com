using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "B2BFiyat",
                table: "Urunler",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "IskontoOrani",
                table: "AspNetUsers",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

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
                columns: new[] { "ConcurrencyStamp", "IskontoOrani", "PasswordHash", "SecurityStamp" },
                values: new object[] { "36f5386a-a98b-4b36-8ef6-a81f0a802d4f", 0m, "AQAAAAIAAYagAAAAEMrceIxLYAfRuqwND5ZHOWDSJQRzgjtLKMIjU5hYV+ZxYCTS4WJARf/j2xxxe3rm4w==", "a711205b-42c8-4a3a-a67f-c95eba76b490" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "B2BFiyat",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "IskontoOrani",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "34123bb9-c222-4e60-aaf4-078c375acf7a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "84e28a55-0024-49c5-8832-bec88e5fde67");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "2160d406-0415-4355-a5e4-148a37387ddd");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "70cee36f-3609-44cf-b495-141f9e882be6", "AQAAAAIAAYagAAAAEDR6H0wf+42t2xcCBeOMl40CizdUY/BATdp82yfbH6EQBDV1kBnr3eCr60FiDgnHCA==", "a761479b-03fa-4172-a317-2dfb4a613ebd" });
        }
    }
}
