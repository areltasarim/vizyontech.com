using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig36 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Banner",
                table: "OneCikanUrunler",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Banner",
                table: "OneCikanUrunler");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "a7b0a8f5-3894-401a-9288-869085638e19");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "0874d7eb-8dfa-49d7-af32-38bb86ec70bd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "bf5e282b-b54d-4964-830a-11f06d49dfed");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1ce1fe85-ca71-4688-b8b8-3d2a90937ce1", "AQAAAAIAAYagAAAAEEtTdiNs6Qkmy0o2ldFV+E1teoGPpCpN2FFg2CP3yHd85hUV9rGDhiV01c4x5EiE1A==", "04f796cd-8259-480e-9f81-4934193fe36f" });
        }
    }
}
