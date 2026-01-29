using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig33 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DataSheetId",
                table: "Urunler",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DosyaKategoriId",
                table: "Dosyalar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "424ceecc-f9e3-44c5-bb34-72262bc41bde");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "1cbf4355-c72c-4d75-87ff-71e6d4f953c1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "5799f651-8b3d-4a5b-ad35-ed77f129d334");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "43f2eb32-923c-4626-b215-1fec3a4aee37", "AQAAAAIAAYagAAAAEE/+kP0t4H7+QrD/oSWz2A4xR9cqQmND3CC5+De9lLyCBNn3aBzeEPP6SPFWpXO2Cg==", "b9386340-1479-41b3-931f-a0b75c65009d" });

            migrationBuilder.CreateIndex(
                name: "IX_Urunler_DataSheetId",
                table: "Urunler",
                column: "DataSheetId");

            migrationBuilder.CreateIndex(
                name: "IX_Dosyalar_DosyaKategoriId",
                table: "Dosyalar",
                column: "DosyaKategoriId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dosyalar_DosyaKategorileri_DosyaKategoriId",
                table: "Dosyalar",
                column: "DosyaKategoriId",
                principalTable: "DosyaKategorileri",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Urunler_Dosyalar_DataSheetId",
                table: "Urunler",
                column: "DataSheetId",
                principalTable: "Dosyalar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dosyalar_DosyaKategorileri_DosyaKategoriId",
                table: "Dosyalar");

            migrationBuilder.DropForeignKey(
                name: "FK_Urunler_Dosyalar_DataSheetId",
                table: "Urunler");

            migrationBuilder.DropIndex(
                name: "IX_Urunler_DataSheetId",
                table: "Urunler");

            migrationBuilder.DropIndex(
                name: "IX_Dosyalar_DosyaKategoriId",
                table: "Dosyalar");

            migrationBuilder.DropColumn(
                name: "DataSheetId",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "DosyaKategoriId",
                table: "Dosyalar");

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
    }
}
