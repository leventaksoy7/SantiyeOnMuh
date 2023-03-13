using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Models;
using SantiyeOnMuh.WebUI.Models.Modeller;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;

namespace SantiyeOnMuh.WebUI.Controllers
{
    public class NakitController : Controller
    {
        // NESNELER ÜZERİNDEKİ İŞLEMLERİ _ OLAN NESNE ÜZERİNDE YAPIP SONRA AKTARIYORUZ - INJECTION
        private INakitService _nakitService;
        private ISirketService _sirketService;
        private ICariHesapService _cariHesapService;
        private IBankaHesapService _bankaHesapService;
        private IBankaKasaService _bankaKasaService;
        private ICariKasaService _cariKasaService;
        private ISantiyeService _santiyeService;

        public NakitController(
            INakitService nakitService,
            ISirketService sirketService,
            ICariHesapService cariHesapService,
            IBankaHesapService bankaHesapService,
            IBankaKasaService bankaKasaService,
            ICariKasaService cariKasaService,
            ISantiyeService santiyeService)
        {
            this._nakitService = nakitService;
            this._sirketService = sirketService;
            this._cariHesapService = cariHesapService;
            this._bankaHesapService = bankaHesapService;
            this._bankaKasaService = bankaKasaService;
            this._cariKasaService = cariKasaService;
            this._santiyeService = santiyeService;
        }

        public IActionResult Index(int page = 1)
        {
            ViewBag.Sayfa = "NAKİT ÖDEMELER";
            const int pageSize = 10;
            var nakitViewModel = new NakitViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _nakitService.GetCount(null, null, null, true),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = null
                },

                Nakits = _nakitService.GetAll(null, null, null, true, page, pageSize),
            };
            return View(nakitViewModel);
        }

        [HttpGet]
        public IActionResult NakitEkleme()
        {
            ViewBag.Sayfa = "YENİ NAKİT ÖDEMESİ EKLE";

            ViewBag.Sirket = _sirketService.GetAll(true);
            ViewBag.Cari = _cariHesapService.GetAll(null, true);
            ViewBag.Banka = _bankaHesapService.GetAll(true);

            return View(new Nakit());
        }

        [HttpPost]
        public IActionResult NakitEkleme(Nakit nakit, IFormFile? file)
        {
            ViewBag.Sayfa = "YENİ NAKİT ÖDEMESİ EKLE";

            ViewBag.Sirket = _sirketService.GetAll(true);
            ViewBag.Cari = _cariHesapService.GetAll(null, true);
            ViewBag.Banka = _bankaHesapService.GetAll(true);

            if (ModelState.IsValid)
            {
                ENakit _nakit = new ENakit()
                {
                    Tarih = nakit.Tarih,
                    Aciklama = nakit.Aciklama,
                    Tutar = Convert.ToDecimal(nakit.Tutar.Replace(".",",")),
                    ImgUrl = nakit.ImgUrl,
                    BankaKasaKaynak = nakit.BankaKasaKaynak,
                    CariKasaKaynak = nakit.CariKasaKaynak,
                    SistemeGiris = nakit.SistemeGiris,
                    SonGuncelleme = nakit.SonGuncelleme,
                    Durum = nakit.Durum,
                    CariHesapId = nakit.CariHesapId,
                    CariHesap = nakit.CariHesap,
                    SirketId = nakit.SirketId,
                    Sirket = nakit.Sirket,
                    BankaHesapId = nakit.BankaHesapId,
                    BankaHesap = nakit.BankaHesap,
                };

                if (_nakitService.Create(_nakit))
                {
                    ECariKasa _cariKasa = new ECariKasa()
                    {
                        Tarih = nakit.Tarih,
                        Aciklama = nakit.Aciklama,
                        Miktar = 1,
                        BirimFiyat = 1,
                        Borc = Convert.ToDecimal(nakit.Tutar.Replace(".", ",")),
                        Alacak = 0,
                        ImgUrl = null,
                        NakitKaynak = _nakit.Id,
                        CekKaynak = null,
                        CariGiderKalemiId = 2,
                        CariHesapId = nakit.CariHesapId
                    };
                    _cariKasaService.Create(_cariKasa);

                    EBankaKasa _bankaKasa = new EBankaKasa()
                    {
                        Tarih = nakit.Tarih,
                        Aciklama = nakit.Aciklama,
                        Nitelik = "NAKİT ÖDEME",
                        Cikan = Convert.ToDecimal(nakit.Tutar.Replace(".", ",")),
                        Giren = 0,
                        NakitKaynak = _nakit.Id,
                        BankaHesapId = nakit.BankaHesapId
                    };
                    _bankaKasaService.Create(_bankaKasa);

                    #region NAKİT ÖDEME - CARİ HESAP VE BANKA ARASINDAKİ BAĞLANTI
                    var _eklenenNakit = _nakitService.GetById(_nakit.Id);

                    if (_eklenenNakit == null) { return NotFound(); }

                    _eklenenNakit.CariKasaKaynak = _cariKasa.Id;

                    _eklenenNakit.BankaKasaKaynak = _bankaKasa.Id;

                    _nakitService.Update(_eklenenNakit);
                    #endregion

                    CreateMessage($"{_nakit.Tutar} ÖDEMESİ EKLENDİ.", "success");

                    return RedirectToAction("Index");
                };

                CreateMessage(_nakitService.ErrorMessage, "danger");

                return View(nakit);
            }
            return View(nakit);
        }

        [HttpGet]
        public IActionResult NakitGuncelle(int? nakitid)
        {
            ViewBag.Sayfa = "NAKİT ÖDEME BİLGİSİNİ GÜNCELLEME";
            ViewBag.Sirket = _sirketService.GetAll(true);
            ViewBag.Cari = _cariHesapService.GetAll(null, true);
            ViewBag.Banka = _bankaHesapService.GetAll(true);

            if (nakitid == null){return NotFound();}

            ENakit nakit = _nakitService.GetById((int)nakitid);

            if (nakit == null){return NotFound();}

            Nakit _nakit = new Nakit()
            {
                Id = nakit.Id,
                Tarih = nakit.Tarih,
                Aciklama = nakit.Aciklama,
                Tutar = Convert.ToString(nakit.Tutar),
                ImgUrl = nakit.ImgUrl,
                BankaKasaKaynak = nakit.BankaKasaKaynak,
                CariKasaKaynak = nakit.CariKasaKaynak,
                SistemeGiris = nakit.SistemeGiris,
                SonGuncelleme = nakit.SonGuncelleme,
                Durum = nakit.Durum,
                CariHesapId = nakit.CariHesapId,
                CariHesap = nakit.CariHesap,
                SirketId = nakit.SirketId,
                Sirket = nakit.Sirket,
                BankaHesapId = nakit.BankaHesapId,
                BankaHesap = nakit.BankaHesap,
            };

            return View(_nakit);
        }

        [HttpPost]
        public IActionResult NakitGuncelle(Nakit nakit)
        {
            ViewBag.Sayfa = "NAKİT ÖDEME BİLGİSİNİ GÜNCELLEME";
            ViewBag.Sirket = _sirketService.GetAll(true);
            ViewBag.Cari = _cariHesapService.GetAll(null, true);
            ViewBag.Banka = _bankaHesapService.GetAll(true);

            if (!ModelState.IsValid) { return View(nakit); }

            ENakit _nakit = _nakitService.GetById(nakit.Id);

            if (_nakit == null){return NotFound();}

            int? bankakasaid = _nakit.BankaKasaKaynak;
            int? carikasaid = _nakit.CariKasaKaynak;

            if (bankakasaid != null)
            {
                EBankaKasa _bankaKasa = _bankaKasaService.GetById((int)bankakasaid);

                if (_bankaKasa == null){return NotFound();}

                _bankaKasa.Tarih = nakit.Tarih;
                _bankaKasa.Aciklama = nakit.Aciklama;
                _bankaKasa.Nitelik = "NAKİT ÖDEME";
                _bankaKasa.Cikan = Convert.ToDecimal(nakit.Tutar.Replace(".",","));
                _bankaKasa.BankaHesapId = nakit.BankaHesapId;
                _bankaKasa.SonGuncelleme = System.DateTime.Now;

                _bankaKasaService.Update(_bankaKasa);
            }

            if (carikasaid != null)
            {
                ECariKasa _cariKasa = _cariKasaService.GetById((int)carikasaid);

                if (_cariKasa == null){return NotFound();}

                _cariKasa.Tarih = nakit.Tarih;
                _cariKasa.Aciklama = nakit.Aciklama;
                _cariKasa.Miktar = 1;
                _cariKasa.BirimFiyat = 1;
                _cariKasa.Borc = Convert.ToDecimal(nakit.Tutar.Replace(".",","));
                _cariKasa.Alacak = 0;
                _cariKasa.CariHesapId = nakit.CariHesapId;
                _cariKasa.SonGuncelleme = System.DateTime.Now;

                _cariKasaService.Update(_cariKasa);
            }

            _nakit.Tarih = nakit.Tarih;
            _nakit.Aciklama = nakit.Aciklama;
            _nakit.Tutar = Convert.ToDecimal(nakit.Tutar.Replace(".",","));
            _nakit.ImgUrl = nakit.ImgUrl;
            _nakit.SonGuncelleme = System.DateTime.Now;
            _nakit.CariHesapId = nakit.CariHesapId;
            _nakit.SirketId = nakit.SirketId;
            _nakit.BankaHesapId = nakit.BankaHesapId;

            _nakitService.Update(_nakit);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult NakitDetay(int? nakitid)
        {
            ViewBag.Sayfa = "NAKİT DETAYI";

            if (nakitid == null){return NotFound();}

            ENakit nakit = _nakitService.GetById((int)nakitid);

            if (nakit == null){return NotFound();}

            //VİEWDA INCLUDE ÇALIŞMADI - ÇEK İÇİN ÇALIŞTI SORUNU BULAMADIM VİEWBAG İLE VERİYİ ATTIM
            ViewBag.Sirket = _sirketService.GetById(nakit.SirketId).Ad;
            ViewBag.Cari = _cariHesapService.GetById(nakit.CariHesapId).Ad;
            ViewBag.Banka = _bankaHesapService.GetById(nakit.BankaHesapId).HesapAdi;

            Nakit _nakit = new Nakit()
            {
                Id = nakit.Id,
                Tarih = nakit.Tarih,
                Aciklama = nakit.Aciklama,
                Tutar = Convert.ToString(nakit.Tutar),
                ImgUrl = nakit.ImgUrl,
                BankaKasaKaynak = nakit.BankaKasaKaynak,
                CariKasaKaynak = nakit.CariKasaKaynak,
                SistemeGiris = nakit.SistemeGiris,
                SonGuncelleme = nakit.SonGuncelleme,
                Durum = nakit.Durum,
                CariHesapId = nakit.CariHesapId,
                CariHesap = nakit.CariHesap,
                SirketId = nakit.SirketId,
                Sirket = nakit.Sirket,
                BankaHesapId = nakit.BankaHesapId,
                BankaHesap = nakit.BankaHesap,
            };

            return View(_nakit);
        }

        [HttpGet]
        public IActionResult NakitSil(int? nakitid)
        {
            ViewBag.Sayfa = "NAKİT ÖDEMESİNİ SİL";

            if (nakitid == null){return NotFound();}

            ENakit nakit = _nakitService.GetById((int)nakitid);

            if (nakit == null){return NotFound();}

            Nakit _nakit = new Nakit()
            {
                Id = nakit.Id,
                Tarih = nakit.Tarih,
                Aciklama = nakit.Aciklama,
                Tutar = Convert.ToString(nakit.Tutar),
                ImgUrl = nakit.ImgUrl,
                BankaKasaKaynak = nakit.BankaKasaKaynak,
                CariKasaKaynak = nakit.CariKasaKaynak,
                SistemeGiris = nakit.SistemeGiris,
                SonGuncelleme = nakit.SonGuncelleme,
                Durum = nakit.Durum,
                CariHesapId = nakit.CariHesapId,
                CariHesap = nakit.CariHesap,
                SirketId = nakit.SirketId,
                Sirket = nakit.Sirket,
                BankaHesapId = nakit.BankaHesapId,
                BankaHesap = nakit.BankaHesap,
            };

            return View(_nakit);
        }

        [HttpPost]
        public IActionResult NakitSil(Nakit nakit)
        {
            //VİEWDA INCLUDE ÇALIŞMADI - ÇEK İÇİN ÇALIŞTI SORUNU BULAMADIM VİEWBAG İLE VERİYİ ATTIM
            //ViewBag.Sirket = _sirketService.GetById(nakit.SirketId).Ad;
            //ViewBag.Cari = _cariHesapService.GetById(nakit.CariHesapId).Ad;
            //ViewBag.Banka = _bankaHesapService.GetById(nakit.BankaHesapId).HesapAdi;

            ENakit _nakit = _nakitService.GetById(nakit.Id);

            if (_nakit == null){return NotFound();}

            _nakit.SonGuncelleme = System.DateTime.Now;
            _nakit.Durum = false;

            int? bankakasaid = _nakit.BankaKasaKaynak;
            int? carikasaid = _nakit.CariKasaKaynak;

            if (bankakasaid != null)
            {
                EBankaKasa _bankaKasa = _bankaKasaService.GetById((int)bankakasaid);

                if (_bankaKasa == null){return NotFound();}

                _bankaKasa.SonGuncelleme = System.DateTime.Now;
                _bankaKasa.Durum = false;

                _bankaKasaService.Update(_bankaKasa);
            }

            if (carikasaid != null)
            {
                ECariKasa _cariKasa = _cariKasaService.GetById((int)carikasaid);

                if (_cariKasa == null){return NotFound();}

                _cariKasa.SonGuncelleme = System.DateTime.Now;
                _cariKasa.Durum = false;

                _cariKasaService.Update(_cariKasa);
            }

            _nakitService.Update(_nakit);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult NakitGeriYukle(int? nakitid)
        {

            if (nakitid == null) { return NotFound(); }

            ENakit _nakit = _nakitService.GetById((int)nakitid);

            if (_nakit == null) { return NotFound(); }

            _nakit.SonGuncelleme = System.DateTime.Now;
            _nakit.Durum = true;

            int? bankakasaid = _nakit.BankaKasaKaynak;
            int? carikasaid = _nakit.CariKasaKaynak;

            if (bankakasaid != null)
            {
                EBankaKasa _bankaKasa = _bankaKasaService.GetById((int)bankakasaid);

                if (_bankaKasa == null) { return NotFound(); }

                _bankaKasa.SonGuncelleme = System.DateTime.Now;
                _bankaKasa.Durum = true;

                _bankaKasaService.Update(_bankaKasa);
            }

            if (carikasaid != null)
            {
                ECariKasa _cariKasa = _cariKasaService.GetById((int)carikasaid);

                if (_cariKasa == null) { return NotFound(); }

                _cariKasa.SonGuncelleme = System.DateTime.Now;
                _cariKasa.Durum = true;

                _cariKasaService.Update(_cariKasa);
            }

            _nakitService.Update(_nakit);

            return RedirectToAction("Index");
        }
        


        //EXCEL
        public IActionResult NakitExcel()
        {
            var nakitViewModel = new NakitViewListModel()
            {
                Nakits = _nakitService.GetAll(null, null, null, true),
            };

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("NAKİT ÖDEMELER");
                var currentRow = 1;
                decimal toplam = 0;

                #region Header
                worksheet.Cell(currentRow, 1).Value = "TARİH";
                worksheet.Cell(currentRow, 2).Value = "AÇIKLAMA";
                worksheet.Cell(currentRow, 3).Value = "BANKA HESABI";
                worksheet.Cell(currentRow, 4).Value = "FİRMA";
                worksheet.Cell(currentRow, 5).Value = "TUTAR";
                worksheet.Cell(currentRow, 6).Value = "TOPLAM";

                for (int i = 1; i < 7; i++)
                {
                    worksheet.Cell(currentRow, i).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }
                #endregion

                #region Body
                foreach (var nakit in nakitViewModel.Nakits)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = nakit.Tarih;
                    worksheet.Cell(currentRow, 2).Value = nakit.Aciklama;
                    worksheet.Cell(currentRow, 3).Value = nakit.BankaHesap.HesapAdi;
                    worksheet.Cell(currentRow, 4).Value = nakit.CariHesap.Ad;
                    worksheet.Cell(currentRow, 5).Value = nakit.Tutar;

                    toplam = toplam + nakit.Tutar;
                    worksheet.Cell(currentRow, 6).Value = toplam;

                    for (int i = 1; i < 7; i++)
                    {
                        worksheet.Cell(currentRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }

                }
                #endregion

                #region FOOTER
                worksheet.Cell(currentRow + 2, 5).Value = "TOPLAM YAPILAN ÖDEME";
                worksheet.Cell(currentRow + 2, 6).Value = toplam;
                for (int i = 5; i < 7; i++)
                {
                    worksheet.Cell(currentRow + 2, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow + 2, i).Style.Font.Bold = true;
                }

                #endregion

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "NakitOdemeler.xlsx"
                        );
                }
            }
        }

        //ARŞİV
        public IActionResult IndexArsiv(int page = 1)
        {
            ViewBag.Sayfa = "NAKİT ÖDEMELER";
            const int pageSize = 10;
            var nakitViewModel = new NakitViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _nakitService.GetCount(null, null, null, false),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = null
                },

                Nakits = _nakitService.GetAll(null, null, null, false, page, pageSize),
            };
            return View(nakitViewModel);
        }






        #region CARİDEN
        [HttpGet]
        public IActionResult NakitEklemeFromCari(int carihesapid)
        {
            ViewBag.Sayfa = _cariHesapService.GetById((int)carihesapid).Ad + " CARİSİNE YENİ NAKİT ÖDEMESİ GİRİŞİ";
            ViewBag.Sirket = _sirketService.GetAll(true);
            ViewBag.Banka = _bankaHesapService.GetAll(true);

            ViewBag.CariHesapId = carihesapid;

            return View(new Nakit());
        }

        [HttpPost]
        public async Task<IActionResult> NakitEklemeFromCari(Nakit nakit, IFormFile file)
        {
            if (!ModelState.IsValid) { return View(nakit); }

            #region  RESİM VS. EKLENMEMİŞSE SAYFAYA GERİ GİDİYOR, GERİ GİDİLEN SAYFANIN İHTİYACI OLAN BİLGİLER
            ViewBag.Sayfa = _cariHesapService.GetById((int)nakit.CariHesapId).Ad + " CARİSİNE YENİ NAKİT ÖDEMESİ GİRİŞİ";
            ViewBag.Sirket = _sirketService.GetAll(true);
            ViewBag.Banka = _bankaHesapService.GetAll(true);

            ViewBag.CariHesapId = nakit.CariHesapId;
            //ÜSTTEKİ SIFIRDAN EKLEMENİN BİREBİR AYNISI, GEREKLİ BİLGİLER
            #endregion
            #region RESİM EKLEME BÖLÜMÜ
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);

                if (extension == ".jpg" || extension == ".png" || extension == ".pdf")
                {
                    var nakitName = string.Format($"{nakit.Aciklama}{"-"}{Guid.NewGuid()}{extension}");
                    nakit.ImgUrl = nakitName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\NakitResim", nakitName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                else { return View(nakit); }
            }
            //else { return View(nakit); }

            #endregion

            ENakit _nakit = new ENakit()
            {
                Tarih = nakit.Tarih,
                Aciklama = nakit.Aciklama,
                Tutar = Convert.ToDecimal(nakit.Tutar.Replace(".",",")),
                ImgUrl = nakit.ImgUrl,
                BankaKasaKaynak = nakit.BankaKasaKaynak,
                CariKasaKaynak = nakit.CariKasaKaynak,
                SistemeGiris = nakit.SistemeGiris,
                SonGuncelleme = nakit.SonGuncelleme,
                Durum = nakit.Durum,
                CariHesapId = nakit.CariHesapId,
                CariHesap = nakit.CariHesap,
                SirketId = nakit.SirketId,
                Sirket = nakit.Sirket,
                BankaHesapId = nakit.BankaHesapId,
                BankaHesap = nakit.BankaHesap,
            };

            _nakitService.Create(_nakit);
            //CARİ KASA İÇİN ÇEK OLUŞTURULDU VE ÇEK KAYNAĞI İLE EKLENDİ
            String FirmaAdiForAciklama = _cariHesapService.GetById((int)nakit.CariHesapId).Ad;

            ECariKasa _cariKasa = new ECariKasa()
            {
                Tarih = nakit.Tarih,
                Aciklama = FirmaAdiForAciklama + " AİT ÖDEME. " + nakit.Aciklama,
                Miktar = 1,
                BirimFiyat = 1,
                Borc = Convert.ToDecimal(nakit.Tutar.Replace(".",",")),
                Alacak = 0,
                ImgUrl = null,
                NakitKaynak = nakit.Id,
                CariGiderKalemiId = 2,
                CariHesapId = nakit.CariHesapId
            };

            _cariKasaService.Create(_cariKasa);

            //BANKA KASA İÇİN ÇEK OLUŞTURULDU VE ÇEK KAYNAĞI İLE EKLENDİ
            EBankaKasa _bankaKasa = new EBankaKasa()
            {
                Tarih = nakit.Tarih,
                Aciklama = FirmaAdiForAciklama + " AİT ÖDEME. " + nakit.Aciklama,
                Nitelik = "NAKİT ÖDEME",
                Cikan = Convert.ToDecimal(nakit.Tutar.Replace(".",",")),
                Giren = 0,
                NakitKaynak = nakit.Id,
                BankaHesapId = nakit.BankaHesapId
            };

            _bankaKasaService.Create(_bankaKasa);

            //ŞİMDİ BANKA KASA VE CARİ HESAP'a AİT NAKİT ÖDEMESİ KAYNAĞI EKLENİYOR
            ENakit _eklenenNakit = _nakitService.GetById(nakit.Id);

            if (_eklenenNakit == null){return NotFound();}

            _eklenenNakit.CariKasaKaynak = _eklenenNakit.Id;
            _eklenenNakit.BankaKasaKaynak = _eklenenNakit.Id;

            _nakitService.Update(_eklenenNakit);

            return RedirectToAction("CariKasa", "CariKasa", new { carihesapid = _nakit.CariHesapId });
        }
        #endregion








        #region ARASEÇİM
        public IActionResult AraSecim()
        {
            ViewBag.Sayfa = "NAKİT ÖDEMELERİ";

            var nakitViewModel = new NakitViewListModel()
            {
                Santiyes = _santiyeService.GetAll(true),
                BankaHesaps = _bankaHesapService.GetAll(true),
                Sirkets = _sirketService.GetAll(true),
            };

            return View(nakitViewModel);
        }

        public IActionResult NakitSantiye(int santiyeid, int page = 1)
        {
            ViewBag.Sayfa = "NAKİT ÖDEMELER";
            const int pageSize = 10;
            var nakitViewModel = new NakitViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _nakitService.GetCount(santiyeid, null, null, true),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = santiyeid
                },
                Nakits = _nakitService.GetAll(santiyeid, null, null, true, page, pageSize),
            };
            return View(nakitViewModel);
        }

        public IActionResult NakitSirket(int sirketid, int page = 1)
        {
            ViewBag.Sayfa = "NAKİT ÖDEMELER";
            const int pageSize = 10;
            var nakitViewModel = new NakitViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _nakitService.GetCount(null, sirketid, null, true),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = sirketid
                },
                Nakits = _nakitService.GetAll(null, sirketid, null, true, page, pageSize),
            };
            return View(nakitViewModel);
        }

        public IActionResult NakitBanka(int bankahesapid, int page = 1)
        {
            ViewBag.Sayfa = "NAKİT ÖDEMELER";
            const int pageSize = 10;
            var nakitViewModel = new NakitViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _nakitService.GetCount(null, null, bankahesapid, true),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = bankahesapid
                },
                Nakits = _nakitService.GetAll(null, null, bankahesapid, true, page, pageSize),
            };
            return View(nakitViewModel);
        }

        //EXCEL
        public IActionResult NakitSantiye(int santiyeid)
        {
            var nakitViewModel = new NakitViewListModel()
            {
                Nakits = _nakitService.GetAll(santiyeid, null, null, true),
            };

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("NAKİT ÖDEMELER");
                var currentRow = 1;
                decimal toplam = 0;

                #region Header
                worksheet.Cell(currentRow, 1).Value = "TARİH";
                worksheet.Cell(currentRow, 2).Value = "AÇIKLAMA";
                worksheet.Cell(currentRow, 3).Value = "BANKA HESABI";
                worksheet.Cell(currentRow, 4).Value = "FİRMA";
                worksheet.Cell(currentRow, 5).Value = "TUTAR";
                worksheet.Cell(currentRow, 6).Value = "TOPLAM";

                for (int i = 1; i < 7; i++)
                {
                    worksheet.Cell(currentRow, i).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }
                #endregion

                #region Body
                foreach (var nakit in nakitViewModel.Nakits)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = nakit.Tarih;
                    worksheet.Cell(currentRow, 2).Value = nakit.Aciklama;
                    worksheet.Cell(currentRow, 3).Value = nakit.BankaHesap.HesapAdi;
                    worksheet.Cell(currentRow, 4).Value = nakit.CariHesap.Ad;
                    worksheet.Cell(currentRow, 5).Value = nakit.Tutar;

                    toplam = toplam + nakit.Tutar;
                    worksheet.Cell(currentRow, 6).Value = toplam;

                    for (int i = 1; i < 7; i++)
                    {
                        worksheet.Cell(currentRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }

                }
                #endregion

                #region FOOTER
                worksheet.Cell(currentRow + 2, 5).Value = "TOPLAM YAPILAN ÖDEME";
                worksheet.Cell(currentRow + 2, 6).Value = toplam;
                for (int i = 5; i < 7; i++)
                {
                    worksheet.Cell(currentRow + 2, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow + 2, i).Style.Font.Bold = true;
                }

                #endregion

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "NakitOdemeler.xlsx"
                        );
                }
            }
        }

        public IActionResult NakitSirket(int sirketid)
        {
            var nakitViewModel = new NakitViewListModel()
            {
                Nakits = _nakitService.GetAll(null, sirketid, null, true),
            };

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("NAKİT ÖDEMELER");
                var currentRow = 1;
                decimal toplam = 0;

                #region Header
                worksheet.Cell(currentRow, 1).Value = "TARİH";
                worksheet.Cell(currentRow, 2).Value = "AÇIKLAMA";
                worksheet.Cell(currentRow, 3).Value = "BANKA HESABI";
                worksheet.Cell(currentRow, 4).Value = "FİRMA";
                worksheet.Cell(currentRow, 5).Value = "TUTAR";
                worksheet.Cell(currentRow, 6).Value = "TOPLAM";

                for (int i = 1; i < 7; i++)
                {
                    worksheet.Cell(currentRow, i).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }
                #endregion

                #region Body
                foreach (var nakit in nakitViewModel.Nakits)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = nakit.Tarih;
                    worksheet.Cell(currentRow, 2).Value = nakit.Aciklama;
                    worksheet.Cell(currentRow, 3).Value = nakit.BankaHesap.HesapAdi;
                    worksheet.Cell(currentRow, 4).Value = nakit.CariHesap.Ad;
                    worksheet.Cell(currentRow, 5).Value = nakit.Tutar;

                    toplam = toplam + nakit.Tutar;
                    worksheet.Cell(currentRow, 6).Value = toplam;

                    for (int i = 1; i < 7; i++)
                    {
                        worksheet.Cell(currentRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }

                }
                #endregion

                #region FOOTER
                worksheet.Cell(currentRow + 2, 5).Value = "TOPLAM YAPILAN ÖDEME";
                worksheet.Cell(currentRow + 2, 6).Value = toplam;
                for (int i = 5; i < 7; i++)
                {
                    worksheet.Cell(currentRow + 2, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow + 2, i).Style.Font.Bold = true;
                }

                #endregion

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "NakitOdemeler.xlsx"
                        );
                }
            }
        }

        public IActionResult NakitBankaHesap(int bankahesapid)
        {
            var nakitViewModel = new NakitViewListModel()
            {
                Nakits = _nakitService.GetAll(null, null, bankahesapid, true),
            };

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("NAKİT ÖDEMELER");
                var currentRow = 1;
                decimal toplam = 0;

                #region Header
                worksheet.Cell(currentRow, 1).Value = "TARİH";
                worksheet.Cell(currentRow, 2).Value = "AÇIKLAMA";
                worksheet.Cell(currentRow, 3).Value = "BANKA HESABI";
                worksheet.Cell(currentRow, 4).Value = "FİRMA";
                worksheet.Cell(currentRow, 5).Value = "TUTAR";
                worksheet.Cell(currentRow, 6).Value = "TOPLAM";

                for (int i = 1; i < 7; i++)
                {
                    worksheet.Cell(currentRow, i).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }
                #endregion

                #region Body
                foreach (var nakit in nakitViewModel.Nakits)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = nakit.Tarih;
                    worksheet.Cell(currentRow, 2).Value = nakit.Aciklama;
                    worksheet.Cell(currentRow, 3).Value = nakit.BankaHesap.HesapAdi;
                    worksheet.Cell(currentRow, 4).Value = nakit.CariHesap.Ad;
                    worksheet.Cell(currentRow, 5).Value = nakit.Tutar;

                    toplam = toplam + nakit.Tutar;
                    worksheet.Cell(currentRow, 6).Value = toplam;

                    for (int i = 1; i < 7; i++)
                    {
                        worksheet.Cell(currentRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }

                }
                #endregion

                #region FOOTER
                worksheet.Cell(currentRow + 2, 5).Value = "TOPLAM YAPILAN ÖDEME";
                worksheet.Cell(currentRow + 2, 6).Value = toplam;
                for (int i = 5; i < 7; i++)
                {
                    worksheet.Cell(currentRow + 2, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow + 2, i).Style.Font.Bold = true;
                }

                #endregion

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "NakitOdemeler.xlsx"
                        );
                }
            }
        }
        #endregion



        private void CreateMessage(string message, string alertType)
        {
            AlertMessage msg = new AlertMessage()
            {
                //Message = $"{_bankaHesap.HesapAdi} HESABI AÇILDI.",
                Message = message,
                AlertType = alertType
            };

            TempData["message"] = JsonConvert.SerializeObject(msg);
        }
    }
}
