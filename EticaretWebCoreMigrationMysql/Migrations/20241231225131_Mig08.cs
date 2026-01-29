using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig08 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Durum",
                table: "OneCikanUrunler");

            migrationBuilder.DropColumn(
                name: "Sira",
                table: "OneCikanUrunler");

            migrationBuilder.AddColumn<int>(
                name: "Durum",
                table: "ModullerTranslate",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "6e983159-aac9-42da-8698-c58ee4b922f7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "245c5ac8-b863-49ab-9e7a-d275a386f6d1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "781a530d-8660-4dbc-a63e-5e3a9fa648f3");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2b51129e-55d7-4883-ad36-59f7fe225cda", "AQAAAAIAAYagAAAAENGMQFhGC9q7lkklaLuzBdNAa5qBQu5tNfZJIZfkwKJ7k/S1W2wnI3D01xRjEYb8kQ==", "1e5727df-a8b8-4087-a7df-e5a32c5ab1cd" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Durum",
                table: "ModullerTranslate");

            migrationBuilder.AddColumn<int>(
                name: "Durum",
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

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "f5a99a0f-371c-4903-9676-67894d06a289");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "d1952cdf-cd8e-43fb-bf44-738fc691a59a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "482b88cf-51a1-4e27-ab53-41193f68e0a8");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6cbae8da-5995-4a42-b1b7-ca3d110559f9", "AQAAAAIAAYagAAAAEHRVugpVTXW7e5p5yqIyT34YFHTSiQ8yq48NQ+UTOG1/EGyBfN7T0r9h5URfyiZI/A==", "2ee9f5e0-6fa0-41b6-a2e4-738e1e4450db" });
        }
    }
}
