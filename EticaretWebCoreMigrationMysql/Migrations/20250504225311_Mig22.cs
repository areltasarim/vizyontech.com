using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "OdenenTutar",
                table: "CariOdeme",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "b5e13015-3f14-41b0-a22a-64199259a036");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "1bbb770d-5f8f-46c0-95c1-9b46439de541");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "3f2bb181-7093-4c85-8518-50e0f728d655");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "95562ebc-0fe0-476c-8412-a91a33526809", "AQAAAAIAAYagAAAAECWLGrXfBH4YhZFpR5lj7ouiU+/sWC4LZ85gpzDr+RfLVSd+k6kbwzjm3BELfLT3Qg==", "6d162f7b-225c-495b-85db-b39dbcad6127" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "OdenenTutar",
                table: "CariOdeme",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "a8a60e52-c8a1-4613-8aa8-fc38a61b0d2d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "cd53b94c-c852-4b6a-b92d-3277b6dba702");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "230a599f-69f4-4375-9c08-6477bdff7e8c");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c39a1c89-4eac-4865-8caf-7256882bb9e5", "AQAAAAIAAYagAAAAEARhjbn/PNZ3euhZuDpfmsGi5BDFdQpCPyixJDw3aWIwTtQLNNqv/kX6gFBztAU/BQ==", "2bc0ecb5-6405-4c4c-92f8-77da726adc68" });
        }
    }
}
