using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "56e718d8-690c-42d4-b460-6dc1ddad1a8f", "AQAAAAIAAYagAAAAELxht/GuGNrjCp9vUwK4JJCwb6JChwczki6VXMkOWilbzw4S9igUmjoLV9FOFY0zGw==", "364b2552-c268-4378-b1cf-d066bdf20e70" });
        }
    }
}
