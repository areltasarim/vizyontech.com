using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OneCikanUrunResimleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Resim = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    OneCikanUrunId = table.Column<int>(type: "int", nullable: false),
                    DilId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneCikanUrunResimleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OneCikanUrunResimleri_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OneCikanUrunResimleri_OneCikanUrunler_OneCikanUrunId",
                        column: x => x.OneCikanUrunId,
                        principalTable: "OneCikanUrunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.CreateIndex(
                name: "IX_OneCikanUrunResimleri_DilId",
                table: "OneCikanUrunResimleri",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_OneCikanUrunResimleri_OneCikanUrunId",
                table: "OneCikanUrunResimleri",
                column: "OneCikanUrunId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OneCikanUrunResimleri");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "530ab0f7-06a5-4cf0-8a2e-3555c6529a24");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "839ff9ca-673f-4967-9155-f370d633554b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "a39400a9-afe7-4703-82b1-6355b1e81e12");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2f616673-a0e6-43a9-b7cd-7cad24d845c1", "AQAAAAIAAYagAAAAEF+BJFE+LlHl+AzhB0Si7q5VOS1tFB0pzAJ1zjM6aQkodG2M6dDKDi4mkeQNjdnMdg==", "17103de2-9c7c-485b-8527-b2cba0e00c7f" });
        }
    }
}
