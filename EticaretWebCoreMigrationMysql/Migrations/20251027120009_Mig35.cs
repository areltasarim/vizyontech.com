using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig35 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BodyKod",
                table: "SiteAyarlari",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.UpdateData(
                table: "SiteAyarlari",
                keyColumn: "Id",
                keyValue: 1,
                column: "BodyKod",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BodyKod",
                table: "SiteAyarlari");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "5e306776-bbdc-4c4f-8597-7d6f5d1631d6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "2ade198d-4aee-4cbd-8547-b7881963d855");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "b0b898b4-1e85-4897-9088-8f6a4741279f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "33db4330-b433-439c-9266-dab13962c513", "AQAAAAIAAYagAAAAEGzefPnbRRJ77oi6N0GZfFywVjWtsoCDCcvY+4J8LAFqHuBstBpkqsJTF5pBGs+0fg==", "5b0c8208-0994-4233-bb4b-472e9c925f26" });
        }
    }
}
