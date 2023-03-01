using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Models;

namespace SantiyeOnMuh.WebUI.Controllers
{
    public class BankaKasaController : Controller
    {
        // NESNELER ÜZERİNDEKİ İŞLEMLERİ _ OLAN NESNE ÜZERİNDE YAPIP SONRA AKTARIYORUZ - INJECTION
        private IBankaKasaService _bankaKasaService;
        private IBankaHesapService _bankaHesapService;
        private ISantiyeKasaService _santiyeKasaService;
        private ISantiyeService _santiyeService;
        public BankaKasaController(
            IBankaKasaService bankaKasaService,
            IBankaHesapService bankaHesapService,
            ISantiyeService santiyeService,
            ISantiyeKasaService santiyeKasaService)
        {
            this._bankaKasaService = bankaKasaService;
            this._bankaHesapService = bankaHesapService;
            this._santiyeService = santiyeService;
            this._santiyeKasaService = santiyeKasaService;
        }
        public IActionResult BankaKasa(int? bankahesapid, int page = 1)
        {
            ViewBag.Sayfa = "ANA KASA";

            const int pageSize = 10;
            var bankaKasaViewModel = new BankaKasaViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _bankaKasaService.GetCount(bankahesapid, true),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = (int?)bankahesapid
                },

                BankaKasas = _bankaKasaService.GetAll((int?)bankahesapid, true, page, pageSize),
                BankaHesaps = _bankaHesapService.GetAll(true)
            };

            ViewBag.toplamgelir = _bankaKasaService.GetAll((int?)bankahesapid, true).Sum(i => i.Giren);
            ViewBag.toplamgider = _bankaKasaService.GetAll((int?)bankahesapid, true).Sum(i => i.Cikan);

            return View(bankaKasaViewModel);
        }
        [HttpGet]
        public IActionResult BankaKasaEkleme()
        {
            ViewBag.Sayfa = "ANA KASAYA VERİ GİRİŞ";
            ViewBag.bankahesap = _bankaHesapService.GetAll(true);

            return View();
        }
        [HttpPost]
        public IActionResult BankaKasaEkleme(BankaKasa b)
        {
            _bankaKasaService.Create(b);
            return RedirectToAction("BankaKasa");
        }
        [HttpGet]
        public IActionResult BankaKasaGuncelle(int? bankakasaid)
        {
            ViewBag.Sayfa = "ANA KASAYA VERİ GÜNCELLEME";
            ViewBag.bankahesap = _bankaHesapService.GetAll(true);

            if (bankakasaid == null) { return NotFound(); }
            BankaKasa bankaKasa = _bankaKasaService.GetByIdDetay((int)bankakasaid);
            if (bankaKasa == null) { return NotFound(); }

            return View(bankaKasa);
        }
        [HttpPost]
        public IActionResult BankaKasaGuncelle(BankaKasa b)
        {
            var entityBankaKasa = _bankaKasaService.GetByIdDetay(b.Id);
            if (entityBankaKasa == null)
            {
                return NotFound();
            }
            entityBankaKasa.Tarih = b.Tarih;
            entityBankaKasa.Aciklama = b.Aciklama;
            entityBankaKasa.Nitelik = b.Nitelik;
            entityBankaKasa.Giren = b.Giren;
            entityBankaKasa.Cikan = b.Cikan;
            entityBankaKasa.SonGuncelleme = System.DateTime.Now;
            entityBankaKasa.BankaHesapId = b.BankaHesapId;

            _bankaKasaService.Update(entityBankaKasa);
            return RedirectToAction("BankaKasa");
        }
        [HttpGet]
        public IActionResult BankaKasaDetay(int? bankakasaid)
        {
            ViewBag.Sayfa = "DETAY";

            if (bankakasaid == null) { return NotFound(); }
            BankaKasa bankaKasa = _bankaKasaService.GetByIdDetay((int)bankakasaid);
            if (bankaKasa == null) { return NotFound(); }

            return View(bankaKasa);
        }
        [HttpGet]
        public IActionResult BankaKasaSil(int? bankakasaid)
        {
            ViewBag.Sayfa = "ANA KASAYA VERİ GÜNCELLEME";

            if (bankakasaid == null) { return NotFound(); }
            BankaKasa bankaKasa = _bankaKasaService.GetByIdDetay((int)bankakasaid);
            if (bankaKasa == null) { return NotFound(); }

            bankaKasa.SonGuncelleme = System.DateTime.Now;
            bankaKasa.Durum = false;

            _bankaKasaService.Update(bankaKasa);
            return RedirectToAction("BankaKasa");
        }
        //EXCEL//
        public IActionResult BankaKasaExcel(int? bankahesapid)
        {
            ViewBag.Sayfa = "ANA KASA";

            var bankaKasaViewModel = new BankaKasaViewListModel()
            {
                BankaKasas = _bankaKasaService.GetAll((int?)bankahesapid, true),
            };

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ANA KASA");
                var currentRow = 1;

                #region Header
                worksheet.Cell(currentRow, 1).Value = "TARİH";
                worksheet.Cell(currentRow, 2).Value = "AÇIKLAMA";
                worksheet.Cell(currentRow, 3).Value = "HESAP ADI";
                worksheet.Cell(currentRow, 4).Value = "NİTELİK";
                worksheet.Cell(currentRow, 5).Value = "GİREN";
                worksheet.Cell(currentRow, 6).Value = "ÇIKAN";
                worksheet.Cell(currentRow, 7).Value = "BAKİYE";

                for (int i = 1; i < 8; i++)
                {
                    worksheet.Cell(currentRow, i).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }
                #endregion

                #region Body
                foreach (var bankakasa in bankaKasaViewModel.BankaKasas)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = bankakasa.Tarih;
                    worksheet.Cell(currentRow, 2).Value = bankakasa.Aciklama;
                    worksheet.Cell(currentRow, 3).Value = bankakasa.BankaHesap.BankaAdi;
                    worksheet.Cell(currentRow, 4).Value = bankakasa.Nitelik;
                    worksheet.Cell(currentRow, 5).Value = bankakasa.Giren;
                    worksheet.Cell(currentRow, 6).Value = bankakasa.Cikan;

                    for (int i = 1; i < 8; i++)
                    {
                        worksheet.Cell(currentRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }
                }
                #endregion

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "AnaKasa.xlsx"
                        );
                }
            }
        }
        //ARŞİV//
        public IActionResult BankaKasaArsiv(int? bankahesapid, int page = 1)
        {
            ViewBag.Sayfa = "ANA KASA";

            const int pageSize = 10;
            var bankaKasaViewModel = new BankaKasaViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _bankaKasaService.GetCount(bankahesapid, false),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = (int?)bankahesapid
                },

                BankaKasas = _bankaKasaService.GetAll((int?)bankahesapid, false, page, pageSize),
                BankaHesaps = _bankaHesapService.GetAll(false)
            };

            ViewBag.toplamgelir = _bankaKasaService.GetAll((int?)bankahesapid, false).Sum(i => i.Giren);
            ViewBag.toplamgider = _bankaKasaService.GetAll((int?)bankahesapid, false).Sum(i => i.Cikan);

            return View(bankaKasaViewModel);
        }
        //GERİ YÜKLEME//
        [HttpGet]
        public IActionResult BankaKasaGeriYukle(int? bankakasaid)
        {
            ViewBag.Sayfa = "ANA KASAYA VERİ GÜNCELLEME";

            if (bankakasaid == null) { return NotFound(); }
            BankaKasa bankaKasa = _bankaKasaService.GetByIdDetay((int)bankakasaid);
            if (bankaKasa == null) { return NotFound(); }

            bankaKasa.SonGuncelleme = System.DateTime.Now;
            bankaKasa.Durum = true;

            _bankaKasaService.Update(bankaKasa);
            return RedirectToAction("BankaKasa");
        }

        #region BANKALAR ARASI EFT
        [HttpGet]
        public IActionResult BankaKasaEklemeEft()
        {
            ViewBag.Sayfa = "HESAPLAR ARASI PARA TRANSFERİ";
            ViewBag.bankahesap = _bankaHesapService.GetAll(true);

            return View();
        }
        [HttpPost]
        public IActionResult BankaKasaEklemeEFT(BankaKasaEftModel b)
        {
            BankaKasa EFTGonderenHesap = new BankaKasa();
            BankaKasa EFTAlanHesap = new BankaKasa();

            //OTOMATİK AÇIKLAMA YAZDIRMAK İÇİN BANKA İSİMLERİNİ ÇEKİCEM
            var GonderenBankaHesabi = _bankaHesapService.GetById((int)b.GonderenBanka);
            var AliciBankaHesabi = _bankaHesapService.GetById((int)b.AliciBanka);

            var GonderenHesapAdi = GonderenBankaHesabi.BankaAdi;
            var AliciHesapAdi = AliciBankaHesabi.BankaAdi;

            EFTGonderenHesap.Tarih = b.Tarih;
            EFTGonderenHesap.Aciklama = AliciHesapAdi + " HESABINA GÖNDERİLEN EFT" + " - " + b.Aciklama;
            EFTGonderenHesap.Nitelik = "EFT";
            EFTGonderenHesap.Giren = 0;
            EFTGonderenHesap.Cikan = b.Tutar;
            EFTGonderenHesap.BankaHesapId = b.GonderenBanka;

            EFTAlanHesap.Tarih = b.Tarih;
            EFTAlanHesap.Aciklama = GonderenHesapAdi + " HESABINDAN GELEN EFT" + " - " + b.Aciklama;
            EFTAlanHesap.Nitelik = "EFT";
            EFTAlanHesap.Giren = b.Tutar;
            EFTAlanHesap.Cikan = 0;
            EFTAlanHesap.BankaHesapId = b.AliciBanka;

            _bankaKasaService.Create(EFTGonderenHesap);
            _bankaKasaService.Create(EFTAlanHesap);
            return RedirectToAction("BankaKasa");
        }
        #endregion

        #region ŞANTİYE KASA EFT İŞLEMLERİ
        [HttpGet]
        public IActionResult BankaKasaEklemeSantiyeEft()
        {
            ViewBag.Sayfa = "ŞANTİYE KASASINA EFT";
            ViewBag.Santiye = _santiyeService.GetAll(true);
            ViewBag.BankaHesap = _bankaHesapService.GetAll(true);

            return View();
        }
        [HttpPost]
        public IActionResult BankaKasaEklemeSantiyeEft(BankaKasaEftSantiyeModel b)
        {
            //ARA MODELE DAYANARAK BANKA KASA MODELİ TANIMLANDI VE EKLENDİ
            string bankahesapadi = _bankaHesapService.GetById((int)b.BankaHesapId).BankaAdi;
            string santiyeadi = _santiyeService.GetById((int)b.SantiyeId).Ad;

            BankaKasa entityBankaKasa = new BankaKasa()
            {
                Tarih = b.Tarih,
                Aciklama = santiyeadi + " ŞANTİYESİNE GÖNDERİLEN EFT" + " - " + b.Aciklama,
                Nitelik = "ŞANTİYE KASASINA EFT",
                Cikan = b.Tutar,
                BankaHesapId = b.BankaHesapId
            };
            _bankaKasaService.Create(entityBankaKasa);

            //ARA MODELE DAYANARAK ŞANTİYE KASA MODELİ TANIMLANDI VE EKLENDİ
            //EKLENMİŞ BANKA KASA MODELİNDEN KAYNAK ID ÇEKİLEREK ŞANTİYE KASASINA YAZILDI
            SantiyeKasa entitySantiyeKasa = new SantiyeKasa()
            {
                Tarih = b.Tarih,
                Aciklama = bankahesapadi + " HESABINDAN KASAYA GELEN EFT" + " - " + b.Aciklama,
                Kisi = "OFİS",
                No = "YOK",
                Gelir = b.Tutar,
                SantiyeId = b.SantiyeId,
                SantiyeGiderKalemiId = 1,
                BankaKasaKaynak = entityBankaKasa.Id
            };
            _santiyeKasaService.Create(entitySantiyeKasa);

            //EKLENMİŞ ŞANTİYE KASA MODELİNDEN KAYNAK ID ÇEKİLEREK BANKA KASASI GÜNCELLENDİ YAZILDI
            var entityBankaKasaKaynakGuncelleme = _bankaKasaService.GetById(entityBankaKasa.Id);

            entityBankaKasaKaynakGuncelleme.SantiyeKasaKaynak = entitySantiyeKasa.Id;

            _bankaKasaService.Update(entityBankaKasaKaynakGuncelleme);

            return RedirectToAction("BankaKasa");
        }
        [HttpGet]
        public IActionResult BankaKasaGuncelleSantiyeEft(int? bankakasasantiyeid)
        {
            ViewBag.Sayfa = "ŞANTİYE KASASINA EFT GUNCELLEME";
            ViewBag.Santiye = _santiyeService.GetAll(true);
            ViewBag.BankaHesap = _bankaHesapService.GetAll(true);

            #region BANKA VE SANTİYE KASA NESNELERİ
            //Gelen ID üzerinden BANKAKASA nesnesini çekiyorum
            if (bankakasasantiyeid == null) { return NotFound(); }
            BankaKasa bankaKasa = _bankaKasaService.GetByIdDetay((int)bankakasasantiyeid);
            if (bankaKasa == null) { return NotFound(); }
            //BankaKasa nesnesi üzerinden santiyekasakaynak id ile ŞANTİYEKASA nesnesini buluyorum
            if (bankakasasantiyeid == null) { return NotFound(); }
            SantiyeKasa santiyeKasa = _santiyeKasaService.GetByIdDetay((int)bankaKasa.SantiyeKasaKaynak);
            if (bankaKasa == null) { return NotFound(); }
            #endregion


            BankaKasaEftSantiyeModel bankaKasaEftSantiyeModel = new BankaKasaEftSantiyeModel()
            {
                Tarih = bankaKasa.Tarih,
                Aciklama = bankaKasa.Aciklama,
                Tutar = bankaKasa.Cikan,
                BankaHesapId = bankaKasa.BankaHesapId,
                SantiyeId = santiyeKasa.SantiyeId,

                BankaKasaId = bankaKasa.Id,
                SantiyeKasaId = bankaKasa.SantiyeKasaKaynak
            };

            return View(bankaKasaEftSantiyeModel);
        }
        [HttpPost]
        public IActionResult BankaKasaGuncelleSantiyeEft(BankaKasaEftSantiyeModel b)
        {
            #region BANKA HESAP ADI VE ŞANTİYE HESAP ADINI BULUYORUZ
            //ARA MODELE DAYANARAK BANKA KASA MODELİ TANIMLANDI VE EKLENDİ
            string bankahesapadi = _bankaHesapService.GetById((int)b.BankaHesapId).BankaAdi;
            //string santiyeadi = _santiyeService.GetById((int)b.SantiyeId).Ad;
            #endregion

            #region BANKAKASA NESNESİ
            //BULUNDU
            BankaKasa entityBankaKasa = _bankaKasaService.GetByIdDetay((int)b.BankaKasaId);
            //GÜNCELLENDİ
            entityBankaKasa.Tarih = b.Tarih;
            entityBankaKasa.Aciklama = b.Aciklama;
            entityBankaKasa.Nitelik = "ŞANTİYE KASASINA EFT";
            entityBankaKasa.Cikan = b.Tutar;
            entityBankaKasa.BankaHesapId = b.BankaHesapId;
            //GÜNCELLEME SONRASI KAYIT EDİLDİ
            _bankaKasaService.Update(entityBankaKasa);
            #endregion

            #region SANTİYEKASA NESNESİ
            //BULUNDU
            SantiyeKasa entitySantiyeKasa = _santiyeKasaService.GetByIdDetay((int)b.SantiyeKasaId);
            //GÜNCELLENDİ
            entitySantiyeKasa.Tarih = b.Tarih;
            entitySantiyeKasa.Aciklama = bankahesapadi + " HESABINDAN KASAYA GELEN EFT" + " - " + b.Aciklama;
            entitySantiyeKasa.Kisi = "OFİS";
            entitySantiyeKasa.No = "YOK";
            entitySantiyeKasa.Gelir = b.Tutar;
            entitySantiyeKasa.SantiyeId = b.SantiyeId;
            entitySantiyeKasa.SantiyeGiderKalemiId = 1;
            entitySantiyeKasa.BankaKasaKaynak = entityBankaKasa.Id;
            //GÜNCELLEME SONRASI KAYIT EDİLDİ
            _santiyeKasaService.Update(entitySantiyeKasa);
            #endregion

            return RedirectToAction("BankaKasa");
        }
        [HttpGet]
        public IActionResult BankaKasaSilSantiyeEft(int? bankakasasantiyeid)
        {
            ViewBag.Sayfa = "ŞANTİYE KASASINA EFT GUNCELLEME";


            #region BANKA VE SANTİYE KASA NESNELERİ
            //Gelen ID üzerinden BANKAKASA nesnesini çekiyorum
            if (bankakasasantiyeid == null) { return NotFound(); }
            BankaKasa bankaKasa = _bankaKasaService.GetByIdDetay((int)bankakasasantiyeid);
            if (bankaKasa == null) { return NotFound(); }
            //BankaKasa nesnesi üzerinden santiyekasakaynak id ile ŞANTİYEKASA nesnesini buluyorum
            if (bankakasasantiyeid == null) { return NotFound(); }
            SantiyeKasa santiyeKasa = _santiyeKasaService.GetByIdDetay((int)bankaKasa.SantiyeKasaKaynak);
            if (bankaKasa == null) { return NotFound(); }
            #endregion

            ViewBag.Santiye = _santiyeService.GetById((int)bankaKasa.SantiyeKasaKaynak).Ad;
            ViewBag.BankaHesap = _bankaHesapService.GetById((int)bankaKasa.BankaHesapId).BankaAdi;

            BankaKasaEftSantiyeModel bankaKasaEftSantiyeModel = new BankaKasaEftSantiyeModel()
            {
                Tarih = bankaKasa.Tarih,
                Aciklama = bankaKasa.Aciklama,
                Tutar = bankaKasa.Cikan,

                BankaHesapId = bankaKasa.BankaHesapId,
                SantiyeId = santiyeKasa.SantiyeId,

                BankaKasaId = bankakasasantiyeid,
                SantiyeKasaId = bankaKasa.SantiyeKasaKaynak
            };

            return View(bankaKasaEftSantiyeModel);
        }
        [HttpPost]
        public IActionResult BankaKasaSilSantiyeEft(BankaKasaEftSantiyeModel b)
        {

            #region BANKAKASA NESNESİ
            //BULUNDU
            BankaKasa entityBankaKasa = _bankaKasaService.GetByIdDetay((int)b.BankaKasaId);
            //GÜNCELLENDİ
            entityBankaKasa.SonGuncelleme = System.DateTime.Now;
            entityBankaKasa.Durum = false;
            //GÜNCELLEME SONRASI KAYIT EDİLDİ
            _bankaKasaService.Update(entityBankaKasa);
            #endregion

            #region SANTİYEKASA NESNESİ
            //BULUNDU
            SantiyeKasa entitySantiyeKasa = _santiyeKasaService.GetByIdDetay((int)b.SantiyeKasaId);
            //GÜNCELLENDİ
            entitySantiyeKasa.SonGuncelleme = System.DateTime.Now;
            entitySantiyeKasa.Durum = false;
            //GÜNCELLEME SONRASI KAYIT EDİLDİ
            _santiyeKasaService.Update(entitySantiyeKasa);
            #endregion

            return RedirectToAction("BankaKasa");
        }
        #endregion
    }
}
