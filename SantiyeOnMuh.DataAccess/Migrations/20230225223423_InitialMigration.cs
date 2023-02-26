using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SantiyeOnMuh.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Santiyes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Durum = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Santiyes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SantiyesKasa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kisi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    No = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gelir = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Gider = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Durum = table.Column<bool>(type: "bit", nullable: false),
                    SistemeGiris = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SonGuncelleme = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SantiyeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SantiyesKasa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SantiyesKasa_Santiyes_SantiyeId",
                        column: x => x.SantiyeId,
                        principalTable: "Santiyes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SantiyesKasa_SantiyeId",
                table: "SantiyesKasa",
                column: "SantiyeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SantiyesKasa");

            migrationBuilder.DropTable(
                name: "Santiyes");
        }
    }
}
