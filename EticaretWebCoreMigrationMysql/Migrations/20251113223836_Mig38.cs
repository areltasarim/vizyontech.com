using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig38 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BannerDurumu",
                table: "OneCikanUrunler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "05172532-e516-4977-8616-ce94999362e6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "ce1d04c3-51f4-4ac0-85f3-bc2078a33d86");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "de2e8371-15b1-492a-b0c9-dc1e0c0bae48");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "da573d38-e6eb-42fd-bb3b-a2d2481279be", "AQAAAAIAAYagAAAAEOXG1J7a1xBvveBpobsg1g2NyeUxzNTFkEKyFNznfHBJ4l9iCX8uT+y5W81ew8Mkyw==", "39d62c8b-cca5-445d-8ff2-3ed61ed6b0d4" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BannerDurumu",
                table: "OneCikanUrunler");

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
    }
}
