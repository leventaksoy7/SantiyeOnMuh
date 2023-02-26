using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SantiyeOnMuh.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "BankaKasaKaynak",
                table: "SantiyesKasa",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SantiyeGiderKalemiId",
                table: "SantiyesKasa",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SantiyesKasa",
                table: "SantiyesKasa",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BankaHesaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HesapAd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankaAd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HesapNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Iban = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Durum = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankaHesaps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CariGiderKalemis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Durum = table.Column<bool>(type: "bit", nullable: false),
                    Tur = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CariGiderKalemis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CariHesaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VergiNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IlgiliKisi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IlgiliKisiTelefon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Odeme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Durum = table.Column<bool>(type: "bit", nullable: false),
                    SantiyeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CariHesaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CariHesaps_Santiyes_SantiyeId",
                        column: x => x.SantiyeId,
                        principalTable: "Santiyes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SantiyeGiderKalemis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Durum = table.Column<bool>(type: "bit", nullable: false),
                    Tur = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SantiyeGiderKalemis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sirkets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VergiNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Durum = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sirkets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankaKasas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nitelik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Giren = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cikan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Durum = table.Column<bool>(type: "bit", nullable: false),
                    CekKaynak = table.Column<int>(type: "int", nullable: true),
                    NakitKaynak = table.Column<int>(type: "int", nullable: true),
                    SantiyeKasaKaynak = table.Column<int>(type: "int", nullable: true),
                    BankaHesapId = table.Column<int>(type: "int", nullable: false),
                    SistemeGiris = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SonGuncelleme = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankaKasas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankaKasas_BankaHesaps_BankaHesapId",
                        column: x => x.BankaHesapId,
                        principalTable: "BankaHesaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CariKasas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Miktar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BirimFiyat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Borc = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Alacak = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CekKaynak = table.Column<int>(type: "int", nullable: true),
                    NakitKaynak = table.Column<int>(type: "int", nullable: true),
                    SistemeGiris = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SonGuncelleme = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Durum = table.Column<bool>(type: "bit", nullable: false),
                    CariKasaGiderKalemiId = table.Column<int>(type: "int", nullable: false),
                    CariHesapKasaGiderKalemiId = table.Column<int>(type: "int", nullable: false),
                    CariHesapId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CariKasas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CariKasas_CariGiderKalemis_CariHesapKasaGiderKalemiId",
                        column: x => x.CariHesapKasaGiderKalemiId,
                        principalTable: "CariGiderKalemis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CariKasas_CariHesaps_CariHesapId",
                        column: x => x.CariHesapId,
                        principalTable: "CariHesaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OdemeCeks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CekNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tutar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankaKasaKaynak = table.Column<int>(type: "int", nullable: true),
                    CariKasaKaynak = table.Column<int>(type: "int", nullable: true),
                    SistemeGiris = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SonGuncelleme = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Durum = table.Column<bool>(type: "bit", nullable: false),
                    OdemeDurumu = table.Column<bool>(type: "bit", nullable: false),
                    CariHesapId = table.Column<int>(type: "int", nullable: false),
                    SirketId = table.Column<int>(type: "int", nullable: false),
                    BankaHesapId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdemeCeks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OdemeCeks_BankaHesaps_BankaHesapId",
                        column: x => x.BankaHesapId,
                        principalTable: "BankaHesaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OdemeCeks_CariHesaps_CariHesapId",
                        column: x => x.CariHesapId,
                        principalTable: "CariHesaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OdemeCeks_Sirkets_SirketId",
                        column: x => x.SirketId,
                        principalTable: "Sirkets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OdemeNakits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tutar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankaKasaKaynak = table.Column<int>(type: "int", nullable: true),
                    CariKasaKaynak = table.Column<int>(type: "int", nullable: true),
                    SistemeGiris = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SonGuncelleme = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Durum = table.Column<bool>(type: "bit", nullable: false),
                    CariHesapId = table.Column<int>(type: "int", nullable: false),
                    SirketId = table.Column<int>(type: "int", nullable: false),
                    BankaHesapId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdemeNakits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OdemeNakits_BankaHesaps_BankaHesapId",
                        column: x => x.BankaHesapId,
                        principalTable: "BankaHesaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OdemeNakits_CariHesaps_CariHesapId",
                        column: x => x.CariHesapId,
                        principalTable: "CariHesaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OdemeNakits_Sirkets_SirketId",
                        column: x => x.SirketId,
                        principalTable: "Sirkets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SantiyesKasa_SantiyeGiderKalemiId",
                table: "SantiyesKasa",
                column: "SantiyeGiderKalemiId");

            migrationBuilder.CreateIndex(
                name: "IX_BankaKasas_BankaHesapId",
                table: "BankaKasas",
                column: "BankaHesapId");

            migrationBuilder.CreateIndex(
                name: "IX_CariHesaps_SantiyeId",
                table: "CariHesaps",
                column: "SantiyeId");

            migrationBuilder.CreateIndex(
                name: "IX_CariKasas_CariHesapId",
                table: "CariKasas",
                column: "CariHesapId");

            migrationBuilder.CreateIndex(
                name: "IX_CariKasas_CariHesapKasaGiderKalemiId",
                table: "CariKasas",
                column: "CariHesapKasaGiderKalemiId");

            migrationBuilder.CreateIndex(
                name: "IX_OdemeCeks_BankaHesapId",
                table: "OdemeCeks",
                column: "BankaHesapId");

            migrationBuilder.CreateIndex(
                name: "IX_OdemeCeks_CariHesapId",
                table: "OdemeCeks",
                column: "CariHesapId");

            migrationBuilder.CreateIndex(
                name: "IX_OdemeCeks_SirketId",
                table: "OdemeCeks",
                column: "SirketId");

            migrationBuilder.CreateIndex(
                name: "IX_OdemeNakits_BankaHesapId",
                table: "OdemeNakits",
                column: "BankaHesapId");

            migrationBuilder.CreateIndex(
                name: "IX_OdemeNakits_CariHesapId",
                table: "OdemeNakits",
                column: "CariHesapId");

            migrationBuilder.CreateIndex(
                name: "IX_OdemeNakits_SirketId",
                table: "OdemeNakits",
                column: "SirketId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SantiyesKasa_SantiyeGiderKalemis_SantiyeGiderKalemiId",
                table: "SantiyesKasa");

            migrationBuilder.DropForeignKey(
                name: "FK_SantiyesKasa_Santiyes_SantiyeId",
                table: "SantiyesKasa");

            migrationBuilder.DropTable(
                name: "BankaKasas");

            migrationBuilder.DropTable(
                name: "CariKasas");

            migrationBuilder.DropTable(
                name: "OdemeCeks");

            migrationBuilder.DropTable(
                name: "OdemeNakits");

            migrationBuilder.DropTable(
                name: "SantiyeGiderKalemis");

            migrationBuilder.DropTable(
                name: "CariGiderKalemis");

            migrationBuilder.DropTable(
                name: "BankaHesaps");

            migrationBuilder.DropTable(
                name: "CariHesaps");

            migrationBuilder.DropTable(
                name: "Sirkets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SantiyesKasa",
                table: "SantiyesKasa");

            migrationBuilder.DropIndex(
                name: "IX_SantiyesKasa_SantiyeGiderKalemiId",
                table: "SantiyesKasa");

            migrationBuilder.DropColumn(
                name: "BankaKasaKaynak",
                table: "SantiyesKasa");

            migrationBuilder.DropColumn(
                name: "SantiyeGiderKalemiId",
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
    }
}
