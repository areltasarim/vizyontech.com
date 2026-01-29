using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColumnDesktop",
                table: "Banner",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ColumnMobil",
                table: "Banner",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "f64209ff-f743-4e8b-833e-f3b3c17868ab");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "f4fe8769-fcc0-4f96-aac4-7c9dd859ee08");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "4f36fa44-0152-4a9b-87dd-91b9df430f19");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "473df48e-269b-4f93-873c-662f7c391cad", "AQAAAAIAAYagAAAAEB6bfRjgft8nKCVjACmpbb4ZqqoXO8D9ooMM743C5DFrV840KB3yQyJc+3N615/c8A==", "95cd3b10-d400-4fa5-a9c2-5176d69bb311" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColumnDesktop",
                table: "Banner");

            migrationBuilder.DropColumn(
                name: "ColumnMobil",
                table: "Banner");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "201173ff-6159-49ad-8f3f-2890fb38dfa5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "8498afbb-a66f-4801-9c22-0a64e5f99410");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "e3ec2f44-4542-4ff9-81be-f8aa1094a13b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a2af42fd-17d3-40fd-8502-17c322340c38", "AQAAAAIAAYagAAAAEGVgT4lV2wkTLnUg2gJc9vYQ8egmwrG+xQW2ZXQCJJWK0GoM7Qk6LHBJYGBl8vTmlg==", "c8964a30-2ea1-44be-8b1e-d1362da53f13" });
        }
    }
}
