using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig07 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sira",
                table: "ModullerTranslate");

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
    }
}
