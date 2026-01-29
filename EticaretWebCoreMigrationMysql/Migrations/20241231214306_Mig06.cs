using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig06 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColumnDesktop",
                table: "OneCikanUrunler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ColumnMobil",
                table: "OneCikanUrunler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sira",
                table: "OneCikanUrunler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sira",
                table: "OneCikanKategoriler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "35a15f10-60cb-4e72-9041-a897e765a79a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "9b1aa454-ef27-4003-a8f0-4599b42fc366");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "ad5bdc00-4f17-4c89-bc0b-dc5e6c417f7c");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "588ccf9a-5bb1-4061-bd4c-20fbb1ba2f7e", "AQAAAAIAAYagAAAAEAlFH1QSJqfOxozz9uA/3mLEDOwjH3X4enPmV+41LpGbv38TKgoeuaAS/U7xkBOJPw==", "9180709a-9cdf-4046-acde-c49623ebaf84" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColumnDesktop",
                table: "OneCikanUrunler");

            migrationBuilder.DropColumn(
                name: "ColumnMobil",
                table: "OneCikanUrunler");

            migrationBuilder.DropColumn(
                name: "Sira",
                table: "OneCikanUrunler");

            migrationBuilder.DropColumn(
                name: "Sira",
                table: "OneCikanKategoriler");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "2c8f5520-d7a2-4116-aa37-5b10fe32fa92");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "031f222a-739c-48a7-a7b7-0feac38193a7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "35717695-ad80-4bcb-9f6c-5ffa9cabf290");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4f602b48-7471-4355-8dbf-dc484cb96b97", "AQAAAAIAAYagAAAAEAhXHbh0z1/mGtDR5Pw32SemYDBoNNS8tboT0bKeUzsdzyg/BgLuA8mxOvozJYT2fQ==", "9ed52f5c-90c5-46f4-b07e-2612dc221c95" });
        }
    }
}
