using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sira",
                table: "OneCikanUrunToUrunler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sira",
                table: "OneCikanUrunToKategoriler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sira",
                table: "OneCikanKategoriToKategoriler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "8c598b42-842b-4074-963e-a64464ec631f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "fdae1ffc-a056-487a-8ff1-20b827fc215a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "6203ed7d-111e-4d30-a1b9-6313826a18d9");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ac0e69d1-64cc-43b8-a730-9be71f6a7250", "AQAAAAIAAYagAAAAEKtJpb2xU1yA/cEesjVAnEQdltOG2vN0LU5lmahIdp7Iz080LUf1HmyTchcHSUHy8w==", "6575e946-b4cb-4f37-8267-0b468fc5b730" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sira",
                table: "OneCikanUrunToUrunler");

            migrationBuilder.DropColumn(
                name: "Sira",
                table: "OneCikanUrunToKategoriler");

            migrationBuilder.DropColumn(
                name: "Sira",
                table: "OneCikanKategoriToKategoriler");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "1841cb2e-0985-4d1f-ad34-8c444627e3ed");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "28dad366-f7a6-4cbd-9d68-b1d6ddd54f34");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "a91f2b88-6761-47fb-900a-f0599ad33f9a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "94e9021e-bd44-467b-b886-5380db26a516", "AQAAAAIAAYagAAAAEK23Ou8Okc7bdtQHTq0tFoPeT15swCiYvga+BQaxXLBgse/5sdBGhC68drsACQkybg==", "6970c443-e39a-4a81-8ed5-6c234b182ee0" });
        }
    }
}
