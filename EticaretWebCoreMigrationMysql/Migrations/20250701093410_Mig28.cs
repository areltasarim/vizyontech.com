using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig28 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UyeKayitTipi",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UyeKayitTipi" },
                values: new object[] { "56204e8a-d24c-4040-a126-69485e979190", "AQAAAAIAAYagAAAAEEgGy3ETKlosTZWcrS9iKOCVBmEC/Jm1cW14qh8D0yT8PZ1/HubzMXs/od0JbJfzbA==", "ce6e7d83-ccd9-4566-b6bc-a9ca7ad0f5f9", 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UyeKayitTipi",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "f653f94b-c1ac-4e8f-9f92-c3ed84a444b5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "1dec1904-59b7-4284-98f1-9de937889033");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "449db0d8-9d1c-4678-a161-c928522e93c9");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "01a54d0f-cef3-4efe-afd5-012c15173545", "AQAAAAIAAYagAAAAEKpeKWIjBsW5/L4rgGLLU/r1du/U61DTIo/XdQ6H+kTAkEn3E11IvrFfEcJ5t3OGaQ==", "e0e97786-d841-4b85-9774-2e281e44ed46" });
        }
    }
}
