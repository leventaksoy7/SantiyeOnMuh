using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SantiyeOnMuh.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OdemeCeks");

            migrationBuilder.DropTable(
                name: "OdemeNakits");

            migrationBuilder.RenameColumn(
                name: "Iban",
                table: "BankaHesaps",
                newName: "IbanNo");

            migrationBuilder.RenameColumn(
                name: "HesapAd",
                table: "BankaHesaps",
                newName: "HesapAdi");

            migrationBuilder.RenameColumn(
                name: "BankaAd",
                table: "BankaHesaps",
                newName: "BankaAdi");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ceks");

            migrationBuilder.DropTable(
                name: "Nakits");

            migrationBuilder.RenameColumn(
                name: "IbanNo",
                table: "BankaHesaps",
                newName: "Iban");

            migrationBuilder.RenameColumn(
                name: "HesapAdi",
                table: "BankaHesaps",
                newName: "HesapAd");

            migrationBuilder.RenameColumn(
                name: "BankaAdi",
                table: "BankaHesaps",
                newName: "BankaAd");

            migrationBuilder.CreateTable(
                name: "OdemeCeks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankaHesapId = table.Column<int>(type: "int", nullable: false),
                    CariHesapId = table.Column<int>(type: "int", nullable: false),
                    SirketId = table.Column<int>(type: "int", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankaKasaKaynak = table.Column<int>(type: "int", nullable: true),
                    CariKasaKaynak = table.Column<int>(type: "int", nullable: true),
                    CekNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Durum = table.Column<bool>(type: "bit", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OdemeDurumu = table.Column<bool>(type: "bit", nullable: false),
                    SistemeGiris = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SonGuncelleme = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tutar = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
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
                    BankaHesapId = table.Column<int>(type: "int", nullable: false),
                    CariHesapId = table.Column<int>(type: "int", nullable: false),
                    SirketId = table.Column<int>(type: "int", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankaKasaKaynak = table.Column<int>(type: "int", nullable: true),
                    CariKasaKaynak = table.Column<int>(type: "int", nullable: true),
                    Durum = table.Column<bool>(type: "bit", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SistemeGiris = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SonGuncelleme = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tutar = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
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
        }
    }
}
