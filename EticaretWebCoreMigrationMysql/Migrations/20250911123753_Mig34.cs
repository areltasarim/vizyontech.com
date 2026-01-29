using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig34 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DosyaAdi",
                table: "DosyaGaleri",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DosyaAdi",
                table: "DosyaGaleri");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "424ceecc-f9e3-44c5-bb34-72262bc41bde");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "1cbf4355-c72c-4d75-87ff-71e6d4f953c1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "5799f651-8b3d-4a5b-ad35-ed77f129d334");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "43f2eb32-923c-4626-b215-1fec3a4aee37", "AQAAAAIAAYagAAAAEE/+kP0t4H7+QrD/oSWz2A4xR9cqQmND3CC5+De9lLyCBNn3aBzeEPP6SPFWpXO2Cg==", "b9386340-1479-41b3-931f-a0b75c65009d" });
        }
    }
}
