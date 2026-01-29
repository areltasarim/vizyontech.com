using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OneCikanUrunToKategoriler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OneCikanKategoriId = table.Column<int>(type: "int", nullable: false),
                    KategoriId = table.Column<int>(type: "int", nullable: false),
                    OneCikanUrunlerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneCikanUrunToKategoriler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OneCikanUrunToKategoriler_Kategoriler_KategoriId",
                        column: x => x.KategoriId,
                        principalTable: "Kategoriler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OneCikanUrunToKategoriler_OneCikanKategoriler_OneCikanKatego~",
                        column: x => x.OneCikanKategoriId,
                        principalTable: "OneCikanKategoriler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OneCikanUrunToKategoriler_OneCikanUrunler_OneCikanUrunlerId",
                        column: x => x.OneCikanUrunlerId,
                        principalTable: "OneCikanUrunler",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "c0eca338-2c62-42bd-92b5-1efda4412da1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "a37c502a-4585-49f4-bfe0-b49dd84eccc3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "9d45f7c2-55e0-46a0-aad5-04a48dbb06f7");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "54068d33-ead0-4342-aed5-64f4a67648d4", "AQAAAAIAAYagAAAAEBQna9IketqdTxKhchh/vpaAFgCy6uE6AIrdAJ1utbPVEph8UlxbpBSLpFsMd7Bmhw==", "90e102d1-3f6d-444d-a51b-4808642527e4" });

            migrationBuilder.CreateIndex(
                name: "IX_OneCikanUrunToKategoriler_KategoriId",
                table: "OneCikanUrunToKategoriler",
                column: "KategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_OneCikanUrunToKategoriler_OneCikanKategoriId",
                table: "OneCikanUrunToKategoriler",
                column: "OneCikanKategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_OneCikanUrunToKategoriler_OneCikanUrunlerId",
                table: "OneCikanUrunToKategoriler",
                column: "OneCikanUrunlerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OneCikanUrunToKategoriler");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "cdb09ca0-ea05-43a9-b153-44077e593974");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "a5ae206c-072f-406d-8f62-d4d0f971a1c3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "19c14e64-a460-4bf9-bc52-160f1cbb1176");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "73aa69df-002a-435b-9348-6eb7a1e3b29c", "AQAAAAIAAYagAAAAEFT11R3F8+ZDVGj1OQwRoGGneflJaxb5JfQRtOlrzUq/PGpNc27RBPB8EtV01AcT4g==", "a85a612e-4fac-41f4-b86c-8d4a9c7af2ee" });
        }
    }
}
