using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig09 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Durum",
                table: "OneCikanKategoriler");

            migrationBuilder.DropColumn(
                name: "Sira",
                table: "OneCikanKategoriler");

            migrationBuilder.DropColumn(
                name: "Durum",
                table: "ModullerTranslate");

            migrationBuilder.DropColumn(
                name: "Sira",
                table: "ModullerTranslate");

            migrationBuilder.AddColumn<int>(
                name: "Durum",
                table: "Moduller",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sira",
                table: "Moduller",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Durum",
                table: "Moduller");

            migrationBuilder.DropColumn(
                name: "Sira",
                table: "Moduller");

            migrationBuilder.AddColumn<int>(
                name: "Durum",
                table: "OneCikanKategoriler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sira",
                table: "OneCikanKategoriler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Durum",
                table: "ModullerTranslate",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sira",
                table: "ModullerTranslate",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "90ac9099-d521-4fa8-bfa3-5df5feb05d16");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "b8602554-fa61-4afc-8a8a-0468045e8f6d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "a9d5f303-b21d-44cc-bf39-309cd2633428");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4bbecada-2fa5-4210-9ed0-6408f716748e", "AQAAAAIAAYagAAAAEFi74UT7kDtjlk0eqfY1UyZQT42ZOr6KPDMuJLXIBCAnHZSkjDEh65kZymJ3HjqmQg==", "a3724d15-794c-42fe-9108-0d64659e0fa7" });
        }
    }
}
