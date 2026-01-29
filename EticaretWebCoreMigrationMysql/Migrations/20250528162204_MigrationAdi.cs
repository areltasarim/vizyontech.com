using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class MigrationAdi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Kur",
                table: "Siparisler",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ParaBirimId",
                table: "Siparisler",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParaBirimleriId",
                table: "Siparisler",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "ffeabcc7-6d20-463f-9cbe-8de56d4475c3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "e6e2c9a9-444e-4ddd-b6b8-eec598863114");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "29a01caa-f4a0-4101-a5c1-ff7980b9f5a0");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5cf799ef-eff6-442d-a90a-34dd9c361be2", "AQAAAAIAAYagAAAAEC8KuHE4esaYCX88MzB+g6TX2KQEjqc6WfSCkwtLn3d95JAuHjwHGXLnq2A4RgG/pQ==", "1fc93ade-31e1-4c5b-b145-fa566c4dee20" });

            migrationBuilder.CreateIndex(
                name: "IX_Siparisler_ParaBirimleriId",
                table: "Siparisler",
                column: "ParaBirimleriId");

            migrationBuilder.AddForeignKey(
                name: "FK_Siparisler_ParaBirimleri_ParaBirimleriId",
                table: "Siparisler",
                column: "ParaBirimleriId",
                principalTable: "ParaBirimleri",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Siparisler_ParaBirimleri_ParaBirimleriId",
                table: "Siparisler");

            migrationBuilder.DropIndex(
                name: "IX_Siparisler_ParaBirimleriId",
                table: "Siparisler");

            migrationBuilder.DropColumn(
                name: "Kur",
                table: "Siparisler");

            migrationBuilder.DropColumn(
                name: "ParaBirimId",
                table: "Siparisler");

            migrationBuilder.DropColumn(
                name: "ParaBirimleriId",
                table: "Siparisler");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "013ce2b0-9284-44d1-9764-da5bc870cd65");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "035efaae-8c83-40e7-a120-b6831c09107d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "911b1ccd-9b2c-4cea-8835-4576f25d70d8");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0158e512-ddd7-4fac-b3b6-c88e146ab002", "AQAAAAIAAYagAAAAEEu/YDBRIfgZEt4e+d3Ly3Sw8/oFf3FHQS5jQ4VEV+WtyxphfV9avH2t9h8HPDfEKg==", "05be445e-925b-4627-aa21-25bc3585eab4" });
        }
    }
}
