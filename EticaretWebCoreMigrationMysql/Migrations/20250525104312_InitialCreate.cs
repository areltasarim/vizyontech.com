using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "B2bSifre",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "0303b46e-f42d-41c2-9601-765ebee0a94a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "74e24b6b-5366-4f94-b6a9-496a62949bdb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "89848ac2-8b63-40ff-89ca-b7e8dd9967cb");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "B2bSifre", "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { null, "2f8da17e-6704-4447-8bba-b851d66a8f3b", "AQAAAAIAAYagAAAAEDxRJNNCe9kTwg+MNYivcEiZa9Re6VEFP2HPtRyTNDjd1ONq6WfUnvHDlD/mf6MChg==", "7d508bd1-b6db-4fbc-9f24-e6051d8b070b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "B2bSifre",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "b5e13015-3f14-41b0-a22a-64199259a036");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "1bbb770d-5f8f-46c0-95c1-9b46439de541");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "3f2bb181-7093-4c85-8518-50e0f728d655");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "95562ebc-0fe0-476c-8412-a91a33526809", "AQAAAAIAAYagAAAAECWLGrXfBH4YhZFpR5lj7ouiU+/sWC4LZ85gpzDr+RfLVSd+k6kbwzjm3BELfLT3Qg==", "6d162f7b-225c-495b-85db-b39dbcad6127" });
        }
    }
}
