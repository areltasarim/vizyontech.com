using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                columns: new[] { "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f8ccf358-2a29-48ac-9b21-8b2a9f9a7c2d", "Bayi", "BAYI" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "VergiNumarasi" },
                values: new object[] { "4b2a4377-c1e7-4d9b-aa47-78bd1bcec287", "AQAAAAIAAYagAAAAEGA33tjionnOXdAUZ54YJEf5kxWVWVHA9uQaPVOSJ1zai71jUdSEOp9pV5ycWPaKBw==", "4764403b-081d-49ac-bb30-03ab572f8fc7", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                value: "77779c83-0843-4580-b5dc-6c46b0ab695d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "a7445ccc-0a70-47c5-ab27-7cf75538f755");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2a0cad65-d3e7-4903-9f15-52d4f87b8246", "Uye", "UYE" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "VergiNumarasi" },
                values: new object[] { "947e37c5-437e-4a41-96b8-0100a4478e71", "AQAAAAIAAYagAAAAEGr0D8A5iHUVL45fg9DoDF5X2KrplYxykMRfq0m/nm6kRlATnxQpolAKlkuGEGgHgg==", "087739c3-8e81-4978-9d10-4cfdefc64466", null });
        }
    }
}
