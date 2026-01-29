using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig32 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Resim",
                table: "DosyaGaleri",
                newName: "Dosya");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "2ccfca56-fe61-49dd-913e-748d8d925d2b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "3299ec32-cdbe-4840-bfe5-d480226be814");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "2064c689-ca53-4656-b3cf-234507f264c3");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "57475c45-df50-4d68-ab4f-1a4b6c03ea1d", "AQAAAAIAAYagAAAAECt+fJ3dIBr1p/M9fkNLNbRfxicTS22ziUm4aH3cMR/UwiuX1Fk8OC8OcxrAEKklqA==", "b7f1b50c-a115-4c19-87ba-fe1e10d2513a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Dosya",
                table: "DosyaGaleri",
                newName: "Resim");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "fbb80a62-446b-4f40-802f-88310fb6a990");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "bcb4c11f-7cbc-48a1-88f2-9cd4f4cf8327");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "cac09bc8-0574-4ad2-884e-eb7233fd719a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bb54f6e8-8bcb-48d3-a895-4e37b000b41c", "AQAAAAIAAYagAAAAEJXDrhkPBQ+ihDY2d40Trumon8WQV8mkm8jFqt/RybfZ5yLs1s587MiCTA0Y1oRTWA==", "471fef15-f6df-4986-8072-ab5e9eaec4b5" });
        }
    }
}
