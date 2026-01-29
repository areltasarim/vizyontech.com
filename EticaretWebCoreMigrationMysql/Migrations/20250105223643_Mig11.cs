using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EkiplerTranslate_Ekipler_SabitMenuId",
                table: "EkiplerTranslate");

            migrationBuilder.RenameColumn(
                name: "SabitMenuId",
                table: "EkiplerTranslate",
                newName: "EkipId");

            migrationBuilder.RenameIndex(
                name: "IX_EkiplerTranslate_SabitMenuId",
                table: "EkiplerTranslate",
                newName: "IX_EkiplerTranslate_EkipId");

            migrationBuilder.AddColumn<string>(
                name: "Gorev",
                table: "EkiplerTranslate",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "1841cb2e-0985-4d1f-ad34-8c444627e3ed");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "28dad366-f7a6-4cbd-9d68-b1d6ddd54f34");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "a91f2b88-6761-47fb-900a-f0599ad33f9a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "94e9021e-bd44-467b-b886-5380db26a516", "AQAAAAIAAYagAAAAEK23Ou8Okc7bdtQHTq0tFoPeT15swCiYvga+BQaxXLBgse/5sdBGhC68drsACQkybg==", "6970c443-e39a-4a81-8ed5-6c234b182ee0" });

            migrationBuilder.AddForeignKey(
                name: "FK_EkiplerTranslate_Ekipler_EkipId",
                table: "EkiplerTranslate",
                column: "EkipId",
                principalTable: "Ekipler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EkiplerTranslate_Ekipler_EkipId",
                table: "EkiplerTranslate");

            migrationBuilder.DropColumn(
                name: "Gorev",
                table: "EkiplerTranslate");

            migrationBuilder.RenameColumn(
                name: "EkipId",
                table: "EkiplerTranslate",
                newName: "SabitMenuId");

            migrationBuilder.RenameIndex(
                name: "IX_EkiplerTranslate_EkipId",
                table: "EkiplerTranslate",
                newName: "IX_EkiplerTranslate_SabitMenuId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_EkiplerTranslate_Ekipler_SabitMenuId",
                table: "EkiplerTranslate",
                column: "SabitMenuId",
                principalTable: "Ekipler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
