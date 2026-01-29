using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OdemeDurumu",
                table: "CariOdeme",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "a8a60e52-c8a1-4613-8aa8-fc38a61b0d2d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "cd53b94c-c852-4b6a-b92d-3277b6dba702");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "230a599f-69f4-4375-9c08-6477bdff7e8c");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c39a1c89-4eac-4865-8caf-7256882bb9e5", "AQAAAAIAAYagAAAAEARhjbn/PNZ3euhZuDpfmsGi5BDFdQpCPyixJDw3aWIwTtQLNNqv/kX6gFBztAU/BQ==", "2bc0ecb5-6405-4c4c-92f8-77da726adc68" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OdemeDurumu",
                table: "CariOdeme");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "646aa401-0439-4cf4-ad3a-2ea9c1b84d7f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "9ce993ac-c490-4e13-acc5-fa306bcc2ceb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "f78eb353-3151-4b57-9a09-08075bd762bd");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f6de1b7d-9fdc-41d1-889b-205249e5a532", "AQAAAAIAAYagAAAAEFKeJ62eXiVQH0uRLKmh2p7Bh71/m9XkziHz8zlfsJTBT0Q0+1rqkYtYfNGE0w5z1w==", "b80300dc-f2aa-4eb9-abd2-1f6f034050e0" });
        }
    }
}
