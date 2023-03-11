using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SantiyeOnMuh.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class BuildUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankaHesaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HesapAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankaAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HesapNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IbanNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Durum = table.Column<bool>(type: "bit", nullable: false),
                    Tur = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CariGiderKalemis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SantiyeGiderKalemis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Durum = table.Column<bool>(type: "bit", nullable: false),
                    Tur = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SantiyeGiderKalemis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Santiyes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Durum = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Santiyes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sirkets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VergiNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "CariHesaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VergiNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IlgiliKisi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IlgiliKisiTelefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Odeme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vade = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "SantiyesKasas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kisi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    No = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gelir = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Gider = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Durum = table.Column<bool>(type: "bit", nullable: false),
                    BankaKasaKaynak = table.Column<int>(type: "int", nullable: true),
                    SistemeGiris = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SonGuncelleme = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SantiyeGiderKalemiId = table.Column<int>(type: "int", nullable: false),
                    SantiyeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SantiyesKasas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SantiyesKasas_SantiyeGiderKalemis_SantiyeGiderKalemiId",
                        column: x => x.SantiyeGiderKalemiId,
                        principalTable: "SantiyeGiderKalemis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SantiyesKasas_Santiyes_SantiyeId",
                        column: x => x.SantiyeId,
                        principalTable: "Santiyes",
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
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CekKaynak = table.Column<int>(type: "int", nullable: true),
                    NakitKaynak = table.Column<int>(type: "int", nullable: true),
                    SistemeGiris = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SonGuncelleme = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Durum = table.Column<bool>(type: "bit", nullable: false),
                    CariGiderKalemiId = table.Column<int>(type: "int", nullable: false),
                    CariHesapId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CariKasas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CariKasas_CariGiderKalemis_CariGiderKalemiId",
                        column: x => x.CariGiderKalemiId,
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
                name: "Ceks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CekNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tutar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Ceks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ceks_BankaHesaps_BankaHesapId",
                        column: x => x.BankaHesapId,
                        principalTable: "BankaHesaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ceks_CariHesaps_CariHesapId",
                        column: x => x.CariHesapId,
                        principalTable: "CariHesaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ceks_Sirkets_SirketId",
                        column: x => x.SirketId,
                        principalTable: "Sirkets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Nakits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tutar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Nakits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nakits_BankaHesaps_BankaHesapId",
                        column: x => x.BankaHesapId,
                        principalTable: "BankaHesaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Nakits_CariHesaps_CariHesapId",
                        column: x => x.CariHesapId,
                        principalTable: "CariHesaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Nakits_Sirkets_SirketId",
                        column: x => x.SirketId,
                        principalTable: "Sirkets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankaKasas_BankaHesapId",
                table: "BankaKasas",
                column: "BankaHesapId");

            migrationBuilder.CreateIndex(
                name: "IX_CariHesaps_SantiyeId",
                table: "CariHesaps",
                column: "SantiyeId");

            migrationBuilder.CreateIndex(
                name: "IX_CariKasas_CariGiderKalemiId",
                table: "CariKasas",
                column: "CariGiderKalemiId");

            migrationBuilder.CreateIndex(
                name: "IX_CariKasas_CariHesapId",
                table: "CariKasas",
                column: "CariHesapId");

            migrationBuilder.CreateIndex(
                name: "IX_Ceks_BankaHesapId",
                table: "Ceks",
                column: "BankaHesapId");

            migrationBuilder.CreateIndex(
                name: "IX_Ceks_CariHesapId",
                table: "Ceks",
                column: "CariHesapId");

            migrationBuilder.CreateIndex(
                name: "IX_Ceks_SirketId",
                table: "Ceks",
                column: "SirketId");

            migrationBuilder.CreateIndex(
                name: "IX_Nakits_BankaHesapId",
                table: "Nakits",
                column: "BankaHesapId");

            migrationBuilder.CreateIndex(
                name: "IX_Nakits_CariHesapId",
                table: "Nakits",
                column: "CariHesapId");

            migrationBuilder.CreateIndex(
                name: "IX_Nakits_SirketId",
                table: "Nakits",
                column: "SirketId");

            migrationBuilder.CreateIndex(
                name: "IX_SantiyesKasas_SantiyeGiderKalemiId",
                table: "SantiyesKasas",
                column: "SantiyeGiderKalemiId");

            migrationBuilder.CreateIndex(
                name: "IX_SantiyesKasas_SantiyeId",
                table: "SantiyesKasas",
                column: "SantiyeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankaKasas");

            migrationBuilder.DropTable(
                name: "CariKasas");

            migrationBuilder.DropTable(
                name: "Ceks");

            migrationBuilder.DropTable(
                name: "Nakits");

            migrationBuilder.DropTable(
                name: "SantiyesKasas");

            migrationBuilder.DropTable(
                name: "CariGiderKalemis");

            migrationBuilder.DropTable(
                name: "BankaHesaps");

            migrationBuilder.DropTable(
                name: "CariHesaps");

            migrationBuilder.DropTable(
                name: "Sirkets");

            migrationBuilder.DropTable(
                name: "SantiyeGiderKalemis");

            migrationBuilder.DropTable(
                name: "Santiyes");
        }
    }
}
