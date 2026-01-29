using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig30 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "KdvOrani",
                table: "Kdv",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.CreateTable(
                name: "DosyaKategorileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DosyaKategorileri", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DosyaKategorileriTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KategoriAdi = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    DosyaKategoriId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DosyaKategorileriTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DosyaKategorileriTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DosyaKategorileriTranslate_DosyaKategorileri_DosyaKategoriId",
                        column: x => x.DosyaKategoriId,
                        principalTable: "DosyaKategorileri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.CreateIndex(
                name: "IX_DosyaKategorileriTranslate_DilId",
                table: "DosyaKategorileriTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_DosyaKategorileriTranslate_DosyaKategoriId",
                table: "DosyaKategorileriTranslate",
                column: "DosyaKategoriId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DosyaKategorileriTranslate");

            migrationBuilder.DropTable(
                name: "DosyaKategorileri");

            migrationBuilder.AlterColumn<double>(
                name: "KdvOrani",
                table: "Kdv",
                type: "double",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

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
    }
}
