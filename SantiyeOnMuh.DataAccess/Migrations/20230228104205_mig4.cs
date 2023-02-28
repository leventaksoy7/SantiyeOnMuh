using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SantiyeOnMuh.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CariKasas_CariGiderKalemis_CariHesapKasaGiderKalemiId",
                table: "CariKasas");

            migrationBuilder.DropForeignKey(
                name: "FK_SantiyesKasa_SantiyeGiderKalemis_SantiyeGiderKalemiId",
                table: "SantiyesKasa");

            migrationBuilder.DropForeignKey(
                name: "FK_SantiyesKasa_Santiyes_SantiyeId",
                table: "SantiyesKasa");

            migrationBuilder.DropIndex(
                name: "IX_CariKasas_CariHesapKasaGiderKalemiId",
                table: "CariKasas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SantiyesKasa",
                table: "SantiyesKasa");

            migrationBuilder.DropColumn(
                name: "CariHesapKasaGiderKalemiId",
                table: "CariKasas");

            migrationBuilder.RenameTable(
                name: "SantiyesKasa",
                newName: "SantiyesKasas");

            migrationBuilder.RenameColumn(
                name: "CariKasaGiderKalemiId",
                table: "CariKasas",
                newName: "CariGiderKalemiId");

            migrationBuilder.RenameIndex(
                name: "IX_SantiyesKasa_SantiyeId",
                table: "SantiyesKasas",
                newName: "IX_SantiyesKasas_SantiyeId");

            migrationBuilder.RenameIndex(
                name: "IX_SantiyesKasa_SantiyeGiderKalemiId",
                table: "SantiyesKasas",
                newName: "IX_SantiyesKasas_SantiyeGiderKalemiId");

            migrationBuilder.AlterColumn<int>(
                name: "SantiyeGiderKalemiId",
                table: "SantiyesKasas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SantiyesKasas",
                table: "SantiyesKasas",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CariKasas_CariGiderKalemiId",
                table: "CariKasas",
                column: "CariGiderKalemiId");

            migrationBuilder.AddForeignKey(
                name: "FK_CariKasas_CariGiderKalemis_CariGiderKalemiId",
                table: "CariKasas",
                column: "CariGiderKalemiId",
                principalTable: "CariGiderKalemis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SantiyesKasas_SantiyeGiderKalemis_SantiyeGiderKalemiId",
                table: "SantiyesKasas",
                column: "SantiyeGiderKalemiId",
                principalTable: "SantiyeGiderKalemis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SantiyesKasas_Santiyes_SantiyeId",
                table: "SantiyesKasas",
                column: "SantiyeId",
                principalTable: "Santiyes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CariKasas_CariGiderKalemis_CariGiderKalemiId",
                table: "CariKasas");

            migrationBuilder.DropForeignKey(
                name: "FK_SantiyesKasas_SantiyeGiderKalemis_SantiyeGiderKalemiId",
                table: "SantiyesKasas");

            migrationBuilder.DropForeignKey(
                name: "FK_SantiyesKasas_Santiyes_SantiyeId",
                table: "SantiyesKasas");

            migrationBuilder.DropIndex(
                name: "IX_CariKasas_CariGiderKalemiId",
                table: "CariKasas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SantiyesKasas",
                table: "SantiyesKasas");

            migrationBuilder.RenameTable(
                name: "SantiyesKasas",
                newName: "SantiyesKasa");

            migrationBuilder.RenameColumn(
                name: "CariGiderKalemiId",
                table: "CariKasas",
                newName: "CariKasaGiderKalemiId");

            migrationBuilder.RenameIndex(
                name: "IX_SantiyesKasas_SantiyeId",
                table: "SantiyesKasa",
                newName: "IX_SantiyesKasa_SantiyeId");

            migrationBuilder.RenameIndex(
                name: "IX_SantiyesKasas_SantiyeGiderKalemiId",
                table: "SantiyesKasa",
                newName: "IX_SantiyesKasa_SantiyeGiderKalemiId");

            migrationBuilder.AddColumn<int>(
                name: "CariHesapKasaGiderKalemiId",
                table: "CariKasas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SantiyeGiderKalemiId",
                table: "SantiyesKasa",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SantiyesKasa",
                table: "SantiyesKasa",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CariKasas_CariHesapKasaGiderKalemiId",
                table: "CariKasas",
                column: "CariHesapKasaGiderKalemiId");

            migrationBuilder.AddForeignKey(
                name: "FK_CariKasas_CariGiderKalemis_CariHesapKasaGiderKalemiId",
                table: "CariKasas",
                column: "CariHesapKasaGiderKalemiId",
                principalTable: "CariGiderKalemis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SantiyesKasa_SantiyeGiderKalemis_SantiyeGiderKalemiId",
                table: "SantiyesKasa",
                column: "SantiyeGiderKalemiId",
                principalTable: "SantiyeGiderKalemis",
                principalColumn: "Id");

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
