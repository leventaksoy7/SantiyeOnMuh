using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Models;

namespace SantiyeOnMuh.WebUI.Controllers
{
    public class OdemeNakitController : Controller
    {
        // NESNELER ÜZERİNDEKİ İŞLEMLERİ _ OLAN NESNE ÜZERİNDE YAPIP SONRA AKTARIYORUZ - INJECTION
        private IOdemeNakitService _odemeOdemeNakitService;
        private ISirketService _sirketService;
        private ICariHesapService _cariHesapService;
        private IBankaHesapService _bankaHesapService;
        private IBankaKasaService _bankaKasaService;
        private ICariKasaService _cariKasaService;
        private ISantiyeService _santiyeService;

        public OdemeNakitController(
            IOdemeNakitService odemeOdemeNakitService,
            ISirketService sirketService,
            ICariHesapService cariHesapService,
            IBankaHesapService bankaHesapService,
            IBankaKasaService bankaKasaService,
            ICariKasaService cariKasaService,
            ISantiyeService santiyeService)
        {
            this._odemeOdemeNakitService = odemeOdemeNakitService;
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
            var odemeOdemeNakitViewModel = new OdemeNakitViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _odemeOdemeNakitService.GetCount(null, null, null, true),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = null
                },

                OdemeNakits = _odemeOdemeNakitService.GetAll(null, null, null, true, page, pageSize),
            };
            return View(odemeOdemeNakitViewModel);
        }
        [HttpGet]
        public IActionResult OdemeNakitEkleme()
        {
            ViewBag.Sayfa = "YENİ NAKİT ÖDEMESİ EKLE";

            ViewBag.Sirket = new SelectList(_sirketService.GetAll(true), "Id", "Ad");
            ViewBag.Cari = new SelectList(_cariHesapService.GetAll(null, true), "Id", "Ad");
            ViewBag.Banka = new SelectList(_bankaHesapService.GetAll(true), "Id", "Ad");

            return View(new OdemeNakit());
        }
        [HttpPost]
        public IActionResult OdemeNakitEkleme(OdemeNakit n, IFormFile file)
        {

            ViewBag.Sirket = new SelectList(_sirketService.GetAll(true), "Id", "Ad");
            ViewBag.Cari = new SelectList(_cariHesapService.GetAll(null, true), "Id", "Ad");
            ViewBag.Banka = new SelectList(_bankaHesapService.GetAll(true), "Id", "Ad");

            if (ModelState.IsValid)
            {
                _odemeOdemeNakitService.Create(n);
                //CARİ KASA İÇİN ÇEK OLUŞTURULDU VE ÇEK KAYNAĞI İLE EKLENDİ
                CariKasa entityCariKasa = new CariKasa()
                {
                    Tarih = n.Tarih,
                    Aciklama = n.Aciklama,
                    Miktar = 1,
                    BirimFiyat = 1,
                    Borc = n.Tutar,
                    Alacak = 0,
                    ImgUrl = null,
                    NakitKaynak = n.Id,
                    CekKaynak = null,
                    CariGiderKalemiId = 2,
                    CariHesapId = n.CariHesapId
                };
                _cariKasaService.Create(entityCariKasa);

                //BANKA KASA İÇİN ÇEK OLUŞTURULDU VE ÇEK KAYNAĞI İLE EKLENDİ
                BankaKasa entityBankaKasa = new BankaKasa()
                {
                    Tarih = n.Tarih,
                    Aciklama = n.Aciklama,
                    Nitelik = "NAKİT ÖDEME",
                    Cikan = n.Tutar,
                    Giren = 0,
                    NakitKaynak = n.Id,
                    BankaHesapId = n.BankaHesapId
                };
                _bankaKasaService.Create(entityBankaKasa);

                //ŞİMDİ BANKA KASA VE CARİ HESAP'a AİT NAKİT ÖDEMESİ KAYNAĞI EKLENİYOR
                var EntityEklenenOdemeNakit = _odemeOdemeNakitService.GetById(n.Id);
                if (EntityEklenenOdemeNakit == null)
                {
                    return NotFound();
                }

                EntityEklenenOdemeNakit.CariKasaKaynak = entityCariKasa.Id;
                EntityEklenenOdemeNakit.BankaKasaKaynak = entityBankaKasa.Id;

                _odemeOdemeNakitService.Update(EntityEklenenOdemeNakit);

                return RedirectToAction("BankaKasa", "BankaKasa");
            }

            return View(n);

        }
        [HttpGet]
        public IActionResult OdemeNakitGuncelle(int? odemeOdemeNakitid)
        {
            ViewBag.Sayfa = "NAKİT ÖDEME BİLGİSİNİ GÜNCELLEME";
            ViewBag.Sirket = _sirketService.GetAll(true);
            ViewBag.Cari = _cariHesapService.GetAll(null, true);
            ViewBag.Banka = _bankaHesapService.GetAll(true);

            if (odemeOdemeNakitid == null)
            {
                return NotFound();
            }
            OdemeNakit odemeOdemeNakit = _odemeOdemeNakitService.GetById((int)odemeOdemeNakitid);

            if (odemeOdemeNakit == null)
            {
                return NotFound();
            }
            return View(odemeOdemeNakit);
        }
        [HttpPost]
        public IActionResult OdemeNakitGuncelle(OdemeNakit n)
        {
            var entityOdemeNakit = _odemeOdemeNakitService.GetById(n.Id);

            if (entityOdemeNakit == null)
            {
                return NotFound();
            }

            int? bankakasaid = entityOdemeNakit.BankaKasaKaynak;
            int? carikasaid = entityOdemeNakit.CariKasaKaynak;

            if (bankakasaid != null)
            {
                var entityBankaKasa = _bankaKasaService.GetById((int)bankakasaid);
                if (entityBankaKasa == null)
                {
                    return NotFound();
                }

                entityBankaKasa.Tarih = n.Tarih;
                entityBankaKasa.Aciklama = n.Aciklama;
                entityBankaKasa.Nitelik = "NAKİT ÖDEME";
                entityBankaKasa.Cikan = n.Tutar;
                entityBankaKasa.BankaHesapId = n.BankaHesapId;
                entityBankaKasa.SonGuncelleme = System.DateTime.Now;

                _bankaKasaService.Update(entityBankaKasa);
            }

            if (carikasaid != null)
            {
                var entityCariKasa = _cariKasaService.GetById((int)carikasaid);
                if (entityCariKasa == null)
                {
                    return NotFound();
                }

                entityCariKasa.Tarih = n.Tarih;
                entityCariKasa.Aciklama = n.Aciklama;
                entityCariKasa.Miktar = 1;
                entityCariKasa.BirimFiyat = 1;
                entityCariKasa.Borc = n.Tutar;
                entityCariKasa.Alacak = 0;
                entityCariKasa.CariHesapId = n.CariHesapId;
                entityCariKasa.SonGuncelleme = System.DateTime.Now;

                _cariKasaService.Update(entityCariKasa);
            }

            entityOdemeNakit.Tarih = n.Tarih;
            entityOdemeNakit.Aciklama = n.Aciklama;
            entityOdemeNakit.Tutar = n.Tutar;
            entityOdemeNakit.ImgUrl = n.ImgUrl;
            entityOdemeNakit.SonGuncelleme = System.DateTime.Now;
            entityOdemeNakit.CariHesapId = n.CariHesapId;
            entityOdemeNakit.SirketId = n.SirketId;
            entityOdemeNakit.BankaHesapId = n.BankaHesapId;

            _odemeOdemeNakitService.Update(entityOdemeNakit);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult OdemeNakit(int? odemeOdemeNakitid)
        {
            ViewBag.Sayfa = "NAKİT DETAYI";
            if (odemeOdemeNakitid == null)
            {
                return NotFound();
            }

            OdemeNakit odemeOdemeNakit = _odemeOdemeNakitService.GetById((int)odemeOdemeNakitid);

            if (odemeOdemeNakit == null)
            {
                return NotFound();
            }

            ViewBag.Sirket = _sirketService.GetAll();
            ViewBag.Cari = _cariHesapService.GetAll();
            ViewBag.Banka = _bankaHesapService.GetAll();

            return View(odemeOdemeNakit);
        }
        [HttpGet]
        public IActionResult OdemeNakitSil(int? odemeOdemeNakitid)
        {
            ViewBag.Sayfa = "NAKİT ÖDEMESİNİ SİL";


            if (odemeOdemeNakitid == null)
            {
                return NotFound();
            }
            OdemeNakit odemeOdemeNakit = _odemeOdemeNakitService.GetById((int)odemeOdemeNakitid);

            if (odemeOdemeNakit == null)
            {
                return NotFound();
            }
            return View(odemeOdemeNakit);
        }
        [HttpPost]
        public IActionResult OdemeNakitSil(OdemeNakit n)
        {
            var entityOdemeNakit = _odemeOdemeNakitService.GetById(n.Id);
            if (entityOdemeNakit == null)
            {
                return NotFound();
            }

            entityOdemeNakit.SonGuncelleme = System.DateTime.Now;
            entityOdemeNakit.Durum = false;

            int? bankakasaid = entityOdemeNakit.BankaKasaKaynak;
            int? carikasaid = entityOdemeNakit.CariKasaKaynak;

            if (bankakasaid != null)
            {
                var entityBankaKasa = _bankaKasaService.GetById((int)bankakasaid);
                if (entityBankaKasa == null)
                {
                    return NotFound();
                }

                entityBankaKasa.SonGuncelleme = System.DateTime.Now;
                entityBankaKasa.Durum = false;
                _bankaKasaService.Update(entityBankaKasa);
            }

            if (carikasaid != null)
            {
                var entityCariKasa = _cariKasaService.GetById((int)carikasaid);
                if (entityCariKasa == null)
                {
                    return NotFound();
                }
                entityCariKasa.SonGuncelleme = System.DateTime.Now;
                entityCariKasa.Durum = false;
                _cariKasaService.Update(entityCariKasa);
            }

            _odemeOdemeNakitService.Update(entityOdemeNakit);
            return RedirectToAction("Index");
        }


        //EXCEL
        public IActionResult OdemeNakitExcel()
        {
            var odemeOdemeNakitViewModel = new OdemeNakitViewListModel()
            {
                OdemeNakits = _odemeOdemeNakitService.GetAll(null, null, null, true),
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
                foreach (var odemeOdemeNakit in odemeOdemeNakitViewModel.OdemeNakits)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = odemeOdemeNakit.Tarih;
                    worksheet.Cell(currentRow, 2).Value = odemeOdemeNakit.Aciklama;
                    worksheet.Cell(currentRow, 3).Value = odemeOdemeNakit.BankaHesap.BankaAd;
                    worksheet.Cell(currentRow, 4).Value = odemeOdemeNakit.CariHesap.Ad;
                    worksheet.Cell(currentRow, 5).Value = odemeOdemeNakit.Tutar;

                    toplam = toplam + odemeOdemeNakit.Tutar;
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
                        "OdemeNakitOdemeler.xlsx"
                        );
                }
            }
        }
        //ARŞİV
        public IActionResult IndexArsiv(int page = 1)
        {
            ViewBag.Sayfa = "NAKİT ÖDEMELER";
            const int pageSize = 10;
            var odemeOdemeNakitViewModel = new OdemeNakitViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _odemeOdemeNakitService.GetCount(null, null, null, false),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = null
                },

                OdemeNakits = _odemeOdemeNakitService.GetAll(null, null, null, false, page, pageSize),
            };
            return View(odemeOdemeNakitViewModel);
        }

        #region CARİDEN
        [HttpGet]
        public IActionResult OdemeNakitEklemeFromCari(int carihesapid)
        {
            ViewBag.Sayfa = _cariHesapService.GetById((int)carihesapid).Ad + " CARİSİNE YENİ NAKİT ÖDEMESİ GİRİŞİ";
            ViewBag.Sirket = _sirketService.GetAll(true);
            ViewBag.Banka = _bankaHesapService.GetAll(true);

            ViewBag.CariHesapId = carihesapid;

            return View(new OdemeNakit());
        }
        [HttpPost]
        public async Task<IActionResult> OdemeNakitEklemeFromCari(OdemeNakit n, IFormFile file)
        {
            #region  RESİM VS. EKLENMEMİŞSE SAYFAYA GERİ GİDİYOR, GERİ GİDİLEN SAYFANIN İHTİYACI OLAN BİLGİLER
            ViewBag.Sayfa = _cariHesapService.GetById((int)n.CariHesapId).Ad + " CARİSİNE YENİ NAKİT ÖDEMESİ GİRİŞİ";
            ViewBag.Sirket = _sirketService.GetAll(true);
            ViewBag.Banka = _bankaHesapService.GetAll(true);

            ViewBag.CariHesapId = n.CariHesapId;
            //ÜSTTEKİ SIFIRDAN EKLEMENİN BİREBİR AYNISI, GEREKLİ BİLGİLER
            #endregion
            #region RESİM EKLEME BÖLÜMÜ
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);

                if (extension == ".jpg" || extension == ".png" || extension == ".pdf")
                {
                    var odemeOdemeNakitName = string.Format($"{n.Aciklama}{"-"}{Guid.NewGuid()}{extension}");
                    n.ImgUrl = odemeOdemeNakitName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\OdemeNakitResim", odemeOdemeNakitName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                else { return View(n); }
            }
            else { return View(n); }
            #endregion
            _odemeOdemeNakitService.Create(n);
            //CARİ KASA İÇİN ÇEK OLUŞTURULDU VE ÇEK KAYNAĞI İLE EKLENDİ
            String FirmaAdiForAciklama = _cariHesapService.GetById((int)n.CariHesapId).Ad;

            CariKasa EntityCariKasa = new CariKasa()
            {
                Tarih = n.Tarih,
                Aciklama = FirmaAdiForAciklama + " AİT ÖDEME. " + n.Aciklama,
                Miktar = 1,
                BirimFiyat = 1,
                Borc = n.Tutar,
                Alacak = 0,
                ImgUrl = null,
                NakitKaynak = n.Id,
                CariGiderKalemiId = 2,
                CariHesapId = n.CariHesapId
            };
            _cariKasaService.Create(EntityCariKasa);
            //BANKA KASA İÇİN ÇEK OLUŞTURULDU VE ÇEK KAYNAĞI İLE EKLENDİ
            BankaKasa EntityBankaKasa = new BankaKasa()
            {
                Tarih = n.Tarih,
                Aciklama = FirmaAdiForAciklama + " AİT ÖDEME. " + n.Aciklama,
                Nitelik = "NAKİT ÖDEME",
                Cikan = n.Tutar,
                Giren = 0,
                NakitKaynak = n.Id,
                BankaHesapId = n.BankaHesapId
            };
            _bankaKasaService.Create(EntityBankaKasa);
            //ŞİMDİ BANKA KASA VE CARİ HESAP'a AİT NAKİT ÖDEMESİ KAYNAĞI EKLENİYOR
            var EntityEklenenOdemeNakit = _odemeOdemeNakitService.GetById(n.Id);
            if (EntityEklenenOdemeNakit == null)
            {
                return NotFound();
            }
            EntityEklenenOdemeNakit.CariKasaKaynak = EntityCariKasa.Id;
            EntityEklenenOdemeNakit.BankaKasaKaynak = EntityBankaKasa.Id;

            _odemeOdemeNakitService.Update(EntityEklenenOdemeNakit);

            return RedirectToAction("CariKasa", "CariKasa", new { carihesapid = n.CariHesapId });
        }
        #endregion

        #region ARASEÇİM
        public IActionResult AraSecim()
        {
            ViewBag.Sayfa = "NAKİT ÖDEMELERİ";

            var odemeOdemeNakitViewModel = new OdemeNakitViewListModel()
            {
                Santiyes = _santiyeService.GetAll(true),
                BankaHesaps = _bankaHesapService.GetAll(true),
                Sirkets = _sirketService.GetAll(true),
            };

            return View(odemeOdemeNakitViewModel);
        }
        public IActionResult OdemeNakitSantiye(int santiyeid, int page = 1)
        {
            ViewBag.Sayfa = "NAKİT ÖDEMELER";
            const int pageSize = 10;
            var odemeOdemeNakitViewModel = new OdemeNakitViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _odemeOdemeNakitService.GetCount(santiyeid, null, null, true),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = santiyeid
                },
                OdemeNakits = _odemeOdemeNakitService.GetAll(santiyeid, null, null, true, page, pageSize),
            };
            return View(odemeOdemeNakitViewModel);
        }
        public IActionResult OdemeNakitSirket(int sirketid, int page = 1)
        {
            ViewBag.Sayfa = "NAKİT ÖDEMELER";
            const int pageSize = 10;
            var odemeOdemeNakitViewModel = new OdemeNakitViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _odemeOdemeNakitService.GetCount(null, sirketid, null, true),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = sirketid
                },
                OdemeNakits = _odemeOdemeNakitService.GetAll(null, sirketid, null, true, page, pageSize),
            };
            return View(odemeOdemeNakitViewModel);
        }
        public IActionResult OdemeNakitBanka(int bankahesapid, int page = 1)
        {
            ViewBag.Sayfa = "NAKİT ÖDEMELER";
            const int pageSize = 10;
            var odemeOdemeNakitViewModel = new OdemeNakitViewListModel()
            {
                PageInfo = new PageInfo
                {
                    TotalItem = _odemeOdemeNakitService.GetCount(null, null, bankahesapid, true),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    UrlInfo = bankahesapid
                },
                OdemeNakits = _odemeOdemeNakitService.GetAll(null, null, bankahesapid, true, page, pageSize),
            };
            return View(odemeOdemeNakitViewModel);
        }

        //EXCEL
        public IActionResult OdemeNakitSantiye(int santiyeid)
        {
            var odemeOdemeNakitViewModel = new OdemeNakitViewListModel()
            {
                OdemeNakits = _odemeOdemeNakitService.GetAll(santiyeid, null, null, true),
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
                foreach (var odemeOdemeNakit in odemeOdemeNakitViewModel.OdemeNakits)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = odemeOdemeNakit.Tarih;
                    worksheet.Cell(currentRow, 2).Value = odemeOdemeNakit.Aciklama;
                    worksheet.Cell(currentRow, 3).Value = odemeOdemeNakit.BankaHesap.BankaAd;
                    worksheet.Cell(currentRow, 4).Value = odemeOdemeNakit.CariHesap.Ad;
                    worksheet.Cell(currentRow, 5).Value = odemeOdemeNakit.Tutar;

                    toplam = toplam + odemeOdemeNakit.Tutar;
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
                        "OdemeNakitOdemeler.xlsx"
                        );
                }
            }
        }
        public IActionResult OdemeNakitSirket(int sirketid)
        {
            var odemeOdemeNakitViewModel = new OdemeNakitViewListModel()
            {
                OdemeNakits = _odemeOdemeNakitService.GetAll(null, sirketid, null, true),
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
                foreach (var odemeOdemeNakit in odemeOdemeNakitViewModel.OdemeNakits)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = odemeOdemeNakit.Tarih;
                    worksheet.Cell(currentRow, 2).Value = odemeOdemeNakit.Aciklama;
                    worksheet.Cell(currentRow, 3).Value = odemeOdemeNakit.BankaHesap.BankaAd;
                    worksheet.Cell(currentRow, 4).Value = odemeOdemeNakit.CariHesap.Ad;
                    worksheet.Cell(currentRow, 5).Value = odemeOdemeNakit.Tutar;

                    toplam = toplam + odemeOdemeNakit.Tutar;
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
                        "OdemeNakitOdemeler.xlsx"
                        );
                }
            }
        }
        public IActionResult OdemeNakitBankaHesap(int bankahesapid)
        {
            var odemeOdemeNakitViewModel = new OdemeNakitViewListModel()
            {
                OdemeNakits = _odemeOdemeNakitService.GetAll(null, null, bankahesapid, true),
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
                foreach (var odemeOdemeNakit in odemeOdemeNakitViewModel.OdemeNakits)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = odemeOdemeNakit.Tarih;
                    worksheet.Cell(currentRow, 2).Value = odemeOdemeNakit.Aciklama;
                    worksheet.Cell(currentRow, 3).Value = odemeOdemeNakit.BankaHesap.BankaAd;
                    worksheet.Cell(currentRow, 4).Value = odemeOdemeNakit.CariHesap.Ad;
                    worksheet.Cell(currentRow, 5).Value = odemeOdemeNakit.Tutar;

                    toplam = toplam + odemeOdemeNakit.Tutar;
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
                        "OdemeNakitOdemeler.xlsx"
                        );
                }
            }
        }
        #endregion
    }
}
