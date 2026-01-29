using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig39 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ikon",
                table: "Kategoriler",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "56856464-6cd2-4e08-b1ee-6bc86a18153c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "1f6be6c8-cdd1-42ea-ae31-a66767ba65f0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "1a5877e1-1901-4420-a9ec-ef8bf3a5327a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "37761798-f234-480b-9fed-ca82e93e5649", "AQAAAAIAAYagAAAAEN9KEx64tDQ4sImSyH2tg7mbAi+FUWZiI+cnawarovKrIwvSL28+BHdGB6YR1JawjA==", "c3f1b79e-9714-4f3f-9d38-2978bea76a37" });

            migrationBuilder.UpdateData(
                table: "Kategoriler",
                keyColumn: "Id",
                keyValue: 1,
                column: "Ikon",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ikon",
                table: "Kategoriler");

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
    }
}
