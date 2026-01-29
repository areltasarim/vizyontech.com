using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParaBirimId",
                table: "SiteAyarlari",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "530ab0f7-06a5-4cf0-8a2e-3555c6529a24");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "839ff9ca-673f-4967-9155-f370d633554b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "a39400a9-afe7-4703-82b1-6355b1e81e12");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2f616673-a0e6-43a9-b7cd-7cad24d845c1", "AQAAAAIAAYagAAAAEF+BJFE+LlHl+AzhB0Si7q5VOS1tFB0pzAJ1zjM6aQkodG2M6dDKDi4mkeQNjdnMdg==", "17103de2-9c7c-485b-8527-b2cba0e00c7f" });

            migrationBuilder.UpdateData(
                table: "SiteAyarlari",
                keyColumn: "Id",
                keyValue: 1,
                column: "ParaBirimId",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_SiteAyarlari_ParaBirimId",
                table: "SiteAyarlari",
                column: "ParaBirimId");

            migrationBuilder.AddForeignKey(
                name: "FK_SiteAyarlari_ParaBirimleri_ParaBirimId",
                table: "SiteAyarlari",
                column: "ParaBirimId",
                principalTable: "ParaBirimleri",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SiteAyarlari_ParaBirimleri_ParaBirimId",
                table: "SiteAyarlari");

            migrationBuilder.DropIndex(
                name: "IX_SiteAyarlari_ParaBirimId",
                table: "SiteAyarlari");

            migrationBuilder.DropColumn(
                name: "ParaBirimId",
                table: "SiteAyarlari");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "83a1e43c-5174-4054-8217-3fd0f1e74a76");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "276ee438-6aef-45c9-9b17-91a808e40bd2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "e5dd047b-f71b-4816-b0fa-cf25de6478a2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ba203703-f623-47bf-9f37-fad0ce83f255", "AQAAAAIAAYagAAAAEIYQpJ145yYlcnKHDbRAOb5Y3znNNFRSNFq/JwaL8w1F6wpJZC0l2obHt4MqpNkDbQ==", "b7c7cfe3-1ad1-47c0-879c-39afffe0d4da" });
        }
    }
}
