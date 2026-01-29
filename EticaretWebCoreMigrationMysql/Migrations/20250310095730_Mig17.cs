using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CariKodu",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
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
                column: "ConcurrencyStamp",
                value: "2a0cad65-d3e7-4903-9f15-52d4f87b8246");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CariKodu", "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { null, "947e37c5-437e-4a41-96b8-0100a4478e71", "AQAAAAIAAYagAAAAEGr0D8A5iHUVL45fg9DoDF5X2KrplYxykMRfq0m/nm6kRlATnxQpolAKlkuGEGgHgg==", "087739c3-8e81-4978-9d10-4cfdefc64466" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CariKodu",
                table: "AspNetUsers");

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
    }
}
