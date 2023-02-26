using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SantiyeOnMuh.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SantiyesKasa_Santiyes_SantiyeId",
                table: "SantiyesKasa");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SantiyesKasa",
                table: "SantiyesKasa");

            migrationBuilder.RenameTable(
                name: "SantiyesKasa",
                newName: "SantiyeKasa");

            migrationBuilder.RenameIndex(
                name: "IX_SantiyesKasa_SantiyeId",
                table: "SantiyeKasa",
                newName: "IX_SantiyeKasa_SantiyeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SantiyeKasa",
                table: "SantiyeKasa",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SantiyeKasa_Santiyes_SantiyeId",
                table: "SantiyeKasa",
                column: "SantiyeId",
                principalTable: "Santiyes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SantiyeKasa_Santiyes_SantiyeId",
                table: "SantiyeKasa");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SantiyeKasa",
                table: "SantiyeKasa");

            migrationBuilder.RenameTable(
                name: "SantiyeKasa",
                newName: "SantiyesKasa");

            migrationBuilder.RenameIndex(
                name: "IX_SantiyeKasa_SantiyeId",
                table: "SantiyesKasa",
                newName: "IX_SantiyesKasa_SantiyeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SantiyesKasa",
                table: "SantiyesKasa",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SantiyesKasa_Santiyes_SantiyeId",
                table: "SantiyesKasa",
                column: "SantiyeId",
                principalTable: "Santiyes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
