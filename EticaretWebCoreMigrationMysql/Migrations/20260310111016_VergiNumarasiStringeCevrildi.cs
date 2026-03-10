using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class VergiNumarasiStringeCevrildi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VergiNumarasi",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d7606372-b1a7-47f5-abad-9a1b92e37873");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "641e11e9-df46-4c12-a552-1aef5c666af5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "8c328778-390e-43c7-bbf8-122bd4d8752b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "VergiNumarasi" },
                values: new object[] { "315f5714-92e1-4b01-a5ac-2f97d6d8f68e", "AQAAAAIAAYagAAAAEL8aB72yH6PVn+IHyYMQTal4C2DavyYVyCmHdDWT0NZrQDwijXeRtJucdeQF4BlIhg==", "7a882982-3959-4aaa-9282-f88468f97f7f", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "VergiNumarasi",
                table: "AspNetUsers",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "VergiNumarasi" },
                values: new object[] { "37761798-f234-480b-9fed-ca82e93e5649", "AQAAAAIAAYagAAAAEN9KEx64tDQ4sImSyH2tg7mbAi+FUWZiI+cnawarovKrIwvSL28+BHdGB6YR1JawjA==", "c3f1b79e-9714-4f3f-9d38-2978bea76a37", null });
        }
    }
}
