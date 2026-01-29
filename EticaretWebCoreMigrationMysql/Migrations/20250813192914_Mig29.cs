using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig29 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "B2BFiyat",
                table: "Urunler");

            migrationBuilder.RenameColumn(
                name: "IndirimliFiyat",
                table: "Urunler",
                newName: "SizeOzelFiyat");

            migrationBuilder.RenameColumn(
                name: "Fiyat",
                table: "Urunler",
                newName: "ListeFiyat");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "c186b979-cdda-4a3e-8fd5-7b1d0090607a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "bf9422e1-d387-437a-a8a4-7fb4b3f8429e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "6a60603b-ded5-493f-97cb-a9335c9147ec");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4535e279-2916-4ae2-b373-07f37fa63670", "AQAAAAIAAYagAAAAECi95PJYyJyA/LNIPc1wiL4Me4ptxq4IkZxVZtYbdOiaymtqIxu0eeOE2aOKvm6c+A==", "6f50f60d-d6b0-41bc-b312-df03b044862d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SizeOzelFiyat",
                table: "Urunler",
                newName: "IndirimliFiyat");

            migrationBuilder.RenameColumn(
                name: "ListeFiyat",
                table: "Urunler",
                newName: "Fiyat");

            migrationBuilder.AddColumn<decimal>(
                name: "B2BFiyat",
                table: "Urunler",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "232454a1-3e83-46a0-aeab-b830f1cdccd8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "96fa9e97-9b37-4fa4-98b2-c18cf3fe06d8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "59d1a841-e85e-4be1-9f52-ea251a3e3117");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "56204e8a-d24c-4040-a126-69485e979190", "AQAAAAIAAYagAAAAEEgGy3ETKlosTZWcrS9iKOCVBmEC/Jm1cW14qh8D0yT8PZ1/HubzMXs/od0JbJfzbA==", "ce6e7d83-ccd9-4566-b6bc-a9ca7ad0f5f9" });
        }
    }
}
