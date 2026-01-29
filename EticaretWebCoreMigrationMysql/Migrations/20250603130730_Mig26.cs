using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig26 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CariLimit",
                table: "AspNetUsers",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "b681b26c-523c-4dc2-a1f0-16ebf120dec1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "2834b5c9-7f85-4d74-a8bc-9e8858b34951");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "fe39952d-0440-4b6d-bd07-bff364a552a6");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CariLimit", "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { 0m, "f6660223-3255-4bff-ab78-3dae60dc9917", "AQAAAAIAAYagAAAAEHfyjGgwW3MS0s4S46KMlAhUdM7vgYLUgljuAgQ+ZY/+QahJIR5+3OzET4vQoak4aA==", "34cac66e-11fd-4a97-896d-1cfcc0d627a4" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CariLimit",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "ffeabcc7-6d20-463f-9cbe-8de56d4475c3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "e6e2c9a9-444e-4ddd-b6b8-eec598863114");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "29a01caa-f4a0-4101-a5c1-ff7980b9f5a0");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5cf799ef-eff6-442d-a90a-34dd9c361be2", "AQAAAAIAAYagAAAAEC8KuHE4esaYCX88MzB+g6TX2KQEjqc6WfSCkwtLn3d95JAuHjwHGXLnq2A4RgG/pQ==", "1fc93ade-31e1-4c5b-b145-fa566c4dee20" });
        }
    }
}
