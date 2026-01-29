using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig31 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DosyaTipi",
                table: "Dosyalar");

            migrationBuilder.DropColumn(
                name: "SayfaId",
                table: "Dosyalar");

            migrationBuilder.DropColumn(
                name: "SayfaTipi",
                table: "Dosyalar");

            migrationBuilder.CreateTable(
                name: "DosyaGaleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Resim = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    DosyaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DosyaGaleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DosyaGaleri_Dosyalar_DosyaId",
                        column: x => x.DosyaId,
                        principalTable: "Dosyalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.CreateIndex(
                name: "IX_DosyaGaleri_DosyaId",
                table: "DosyaGaleri",
                column: "DosyaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DosyaGaleri");

            migrationBuilder.AddColumn<int>(
                name: "DosyaTipi",
                table: "Dosyalar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SayfaId",
                table: "Dosyalar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SayfaTipi",
                table: "Dosyalar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "5e7c5496-a0c1-47d1-ac25-e1a4142e5bbe");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "000e9739-68ad-44ea-9960-0a8fdcb28d39");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "eab2cf42-b248-4be9-ade1-ecc9d90e6ebe");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8dde6240-c284-4408-9260-f04f0af90cf4", "AQAAAAIAAYagAAAAEIsXPHKbGg4KZZSMf7ZxtCh41Gzm7SlRSnFAV/tE3JbSxq/Y7iZNYZep8dV+gFLn6g==", "f432250e-c608-4286-9e9d-8a2b473a06f0" });
        }
    }
}
