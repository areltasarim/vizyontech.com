using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OzelFiyatStokSarti",
                table: "Urunler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "2b25f7ce-2dcd-475d-9033-632ae02ea153");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "e63e77e8-3733-447c-ae7d-c8afca7fdca7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "c03d6e17-7a1e-44bb-9d43-eafa5efb8fc7");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e0b87fac-4577-4156-bbec-a0ad91ec4e18", "AQAAAAIAAYagAAAAEJBBvV56vqQdSS1yjPc4e+NDPFgiW/xIh+74icr6uhGPAVGlyElYhn4aDLlYBxxIQw==", "f90d6111-3f3d-440f-9ed9-2dcbb386a893" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OzelFiyatStokSarti",
                table: "Urunler");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "c911ebb3-9df4-4d8a-ace1-7983c4cea28c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "89548ba6-d69b-401d-a1b4-bc2e0bb8f6aa");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "f8d809f7-6185-4130-8b46-f56559cdba28");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a8112551-2d07-45df-a25e-d9fa5fe9a32c", "AQAAAAIAAYagAAAAEHn2AfYNU9xHjw/5WSZruLpRga1YOnMaokYUO9AGmgtym/IAoiJgFzeaI303m4EmFw==", "4bc5ae7b-4826-4067-a179-bd89c3f75f8a" });
        }
    }
}
