using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResimAdi",
                table: "SayfaResimleri",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "34123bb9-c222-4e60-aaf4-078c375acf7a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "84e28a55-0024-49c5-8832-bec88e5fde67");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "2160d406-0415-4355-a5e4-148a37387ddd");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "70cee36f-3609-44cf-b495-141f9e882be6", "AQAAAAIAAYagAAAAEDR6H0wf+42t2xcCBeOMl40CizdUY/BATdp82yfbH6EQBDV1kBnr3eCr60FiDgnHCA==", "a761479b-03fa-4172-a317-2dfb4a613ebd" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResimAdi",
                table: "SayfaResimleri");

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
    }
}
