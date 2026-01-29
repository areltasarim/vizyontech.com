using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig05 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OneCikanUrunToKategoriler_OneCikanKategoriler_OneCikanKatego~",
                table: "OneCikanUrunToKategoriler");

            migrationBuilder.DropForeignKey(
                name: "FK_OneCikanUrunToKategoriler_OneCikanUrunler_OneCikanUrunlerId",
                table: "OneCikanUrunToKategoriler");

            migrationBuilder.DropIndex(
                name: "IX_OneCikanUrunToKategoriler_OneCikanUrunlerId",
                table: "OneCikanUrunToKategoriler");

            migrationBuilder.DropColumn(
                name: "OneCikanUrunlerId",
                table: "OneCikanUrunToKategoriler");

            migrationBuilder.RenameColumn(
                name: "OneCikanKategoriId",
                table: "OneCikanUrunToKategoriler",
                newName: "OneCikanUrunId");

            migrationBuilder.RenameIndex(
                name: "IX_OneCikanUrunToKategoriler_OneCikanKategoriId",
                table: "OneCikanUrunToKategoriler",
                newName: "IX_OneCikanUrunToKategoriler_OneCikanUrunId");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "2c8f5520-d7a2-4116-aa37-5b10fe32fa92");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "031f222a-739c-48a7-a7b7-0feac38193a7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "35717695-ad80-4bcb-9f6c-5ffa9cabf290");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4f602b48-7471-4355-8dbf-dc484cb96b97", "AQAAAAIAAYagAAAAEAhXHbh0z1/mGtDR5Pw32SemYDBoNNS8tboT0bKeUzsdzyg/BgLuA8mxOvozJYT2fQ==", "9ed52f5c-90c5-46f4-b07e-2612dc221c95" });

            migrationBuilder.AddForeignKey(
                name: "FK_OneCikanUrunToKategoriler_OneCikanUrunler_OneCikanUrunId",
                table: "OneCikanUrunToKategoriler",
                column: "OneCikanUrunId",
                principalTable: "OneCikanUrunler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OneCikanUrunToKategoriler_OneCikanUrunler_OneCikanUrunId",
                table: "OneCikanUrunToKategoriler");

            migrationBuilder.RenameColumn(
                name: "OneCikanUrunId",
                table: "OneCikanUrunToKategoriler",
                newName: "OneCikanKategoriId");

            migrationBuilder.RenameIndex(
                name: "IX_OneCikanUrunToKategoriler_OneCikanUrunId",
                table: "OneCikanUrunToKategoriler",
                newName: "IX_OneCikanUrunToKategoriler_OneCikanKategoriId");

            migrationBuilder.AddColumn<int>(
                name: "OneCikanUrunlerId",
                table: "OneCikanUrunToKategoriler",
                type: "int",
                nullable: true);

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
                name: "IX_OneCikanUrunToKategoriler_OneCikanUrunlerId",
                table: "OneCikanUrunToKategoriler",
                column: "OneCikanUrunlerId");

            migrationBuilder.AddForeignKey(
                name: "FK_OneCikanUrunToKategoriler_OneCikanKategoriler_OneCikanKatego~",
                table: "OneCikanUrunToKategoriler",
                column: "OneCikanKategoriId",
                principalTable: "OneCikanKategoriler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OneCikanUrunToKategoriler_OneCikanUrunler_OneCikanUrunlerId",
                table: "OneCikanUrunToKategoriler",
                column: "OneCikanUrunlerId",
                principalTable: "OneCikanUrunler",
                principalColumn: "Id");
        }
    }
}
