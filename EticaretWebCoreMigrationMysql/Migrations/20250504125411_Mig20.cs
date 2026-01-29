using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CariOdeme",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OdemeTarihi = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UyeId = table.Column<int>(type: "int", nullable: true),
                    OdenenTutar = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    ZiraatPaySiparisId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CariOdeme", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CariOdeme_AspNetUsers_UyeId",
                        column: x => x.UyeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.CreateIndex(
                name: "IX_CariOdeme_UyeId",
                table: "CariOdeme",
                column: "UyeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CariOdeme");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "81753c69-0703-4346-a679-4bb23411415f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "d06a28fc-bb08-45f9-b183-d9e4b4d5d0e0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "3c01c01e-c6e3-44e8-9466-ffeb7958d5c4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f7a37ade-5804-48d3-b500-6186dcfb256c", "AQAAAAIAAYagAAAAEE8oVyg88PXUbv2pjM0G8rzkG0UEUI8IXsQBZccMISM0Treh13EHZWJjJ9TsQZSu6Q==", "c6aff42a-f116-4985-84b3-0564a9152abf" });
        }
    }
}
