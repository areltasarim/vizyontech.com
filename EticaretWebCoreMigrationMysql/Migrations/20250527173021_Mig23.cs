using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UyeId",
                table: "CariOdeme",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OpakCariId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlasiyerId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Plasiyer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlasiyerId = table.Column<int>(type: "int", nullable: false),
                    AdSoyad = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Gsm = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Kod = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Grup = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plasiyer", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "bf3527d1-4b38-49e3-8c03-f97e3cb1cc52");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "a7524e14-90a9-43b3-bce6-a733eec68cda");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "f810c35f-22ad-4670-a58a-121a519ae2d5");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "OpakCariId", "PasswordHash", "PlasiyerId", "SecurityStamp" },
                values: new object[] { "56e718d8-690c-42d4-b460-6dc1ddad1a8f", null, "AQAAAAIAAYagAAAAELxht/GuGNrjCp9vUwK4JJCwb6JChwczki6VXMkOWilbzw4S9igUmjoLV9FOFY0zGw==", null, "364b2552-c268-4378-b1cf-d066bdf20e70" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PlasiyerId",
                table: "AspNetUsers",
                column: "PlasiyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Plasiyer_PlasiyerId",
                table: "AspNetUsers",
                column: "PlasiyerId",
                principalTable: "Plasiyer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Plasiyer_PlasiyerId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Plasiyer");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PlasiyerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OpakCariId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PlasiyerId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "UyeId",
                table: "CariOdeme",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "0303b46e-f42d-41c2-9601-765ebee0a94a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "74e24b6b-5366-4f94-b6a9-496a62949bdb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "89848ac2-8b63-40ff-89ca-b7e8dd9967cb");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2f8da17e-6704-4447-8bba-b851d66a8f3b", "AQAAAAIAAYagAAAAEDxRJNNCe9kTwg+MNYivcEiZa9Re6VEFP2HPtRyTNDjd1ONq6WfUnvHDlD/mf6MChg==", "7d508bd1-b6db-4fbc-9f24-e6051d8b070b" });
        }
    }
}
