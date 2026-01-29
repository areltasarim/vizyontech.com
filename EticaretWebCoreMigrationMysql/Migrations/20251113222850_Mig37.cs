using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig37 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BannerUrl",
                table: "OneCikanUrunler",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "e475f29e-ce89-4b8f-bb80-c8b35bc751a1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "93afb470-2ec1-45f0-945f-8931876a8db8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "4cdf0655-52f9-4cbd-9855-ff545fb405a7");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9a455910-c905-4bff-b684-a53995b21c91", "AQAAAAIAAYagAAAAECbYWov2L9QLgTezTr22otTNI7errkbDsR9UZ4zdpR5qxhJ1Zq1TogypAO9m6LN/Tg==", "5fb36bfb-43e1-492f-a956-fc0ecdfe7cba" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BannerUrl",
                table: "OneCikanUrunler");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "7bf362b4-44ca-40ad-b4f5-f9f7cf50756f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "aa7eca7f-08ba-47a4-8b00-0846298a17f5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "85f101c7-c6ed-46fa-9a4f-a0bcb23b04eb");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0d9f89ef-df42-420d-8727-cb9a45c03048", "AQAAAAIAAYagAAAAEPyoUlSZPXQoQHPzsXWLMUFoX8hhYtbFG0s+5t6urPJQwDMNQ1w9pqF2j+jC5/QmOA==", "33e61c5e-773e-4376-9220-fe768eb872a2" });
        }
    }
}
