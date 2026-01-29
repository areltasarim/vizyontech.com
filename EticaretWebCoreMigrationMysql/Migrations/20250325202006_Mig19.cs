using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SayfaAdiAltAciklama2",
                table: "SayfalarTranslate",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "81753c69-0703-4346-a679-4bb23411415f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "d06a28fc-bb08-45f9-b183-d9e4b4d5d0e0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "3c01c01e-c6e3-44e8-9466-ffeb7958d5c4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f7a37ade-5804-48d3-b500-6186dcfb256c", "AQAAAAIAAYagAAAAEE8oVyg88PXUbv2pjM0G8rzkG0UEUI8IXsQBZccMISM0Treh13EHZWJjJ9TsQZSu6Q==", "c6aff42a-f116-4985-84b3-0564a9152abf" });

            migrationBuilder.UpdateData(
                table: "SayfalarTranslate",
                keyColumn: "Id",
                keyValue: 1,
                column: "SayfaAdiAltAciklama2",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SayfaAdiAltAciklama2",
                table: "SayfalarTranslate");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "4f8f0a85-1d8c-41b8-b2ae-48d9a1580510");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "07fb935b-ffc2-4232-a55b-ed79bd804cce");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "f8ccf358-2a29-48ac-9b21-8b2a9f9a7c2d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4b2a4377-c1e7-4d9b-aa47-78bd1bcec287", "AQAAAAIAAYagAAAAEGA33tjionnOXdAUZ54YJEf5kxWVWVHA9uQaPVOSJ1zai71jUdSEOp9pV5ycWPaKBw==", "4764403b-081d-49ac-bb30-03ab572f8fc7" });
        }
    }
}
