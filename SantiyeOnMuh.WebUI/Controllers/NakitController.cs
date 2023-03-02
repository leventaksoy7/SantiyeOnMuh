using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.Entity;
using SantiyeOnMuh.WebUI.Models;

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

            ViewBag.Sirket = new SelectList(_sirketService.GetAll(true), "Id", "Ad");
            ViewBag.Cari = new SelectList(_cariHesapService.GetAll(null, true), "Id", "Ad");
            ViewBag.Banka = new SelectList(_bankaHesapService.GetAll(true), "Id", "Ad");

            return View(new ENakit());
        }
        [HttpPost]
        public IActionResult NakitEkleme(ENakit n, IFormFile file)
        {

            ViewBag.Sirket = new SelectList(_sirketService.GetAll(true), "Id", "Ad");
            ViewBag.Cari = new SelectList(_cariHesapService.GetAll(null, true), "Id", "Ad");
            ViewBag.Banka = new SelectList(_bankaHesapService.GetAll(true), "Id", "Ad");

            if (ModelState.IsValid)
            {
                _nakitService.Create(n);
                //CARİ KASA İÇİN ÇEK OLUŞTURULDU VE ÇEK KAYNAĞI İLE EKLENDİ
                ECariKasa entityCariKasa = new ECariKasa()
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
                EBankaKasa entityBankaKasa = new EBankaKasa()
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
                var EntityEklenenNakit = _nakitService.GetById(n.Id);
                if (EntityEklenenNakit == null)
                {
                    return NotFound();
                }

                EntityEklenenNakit.CariKasaKaynak = entityCariKasa.Id;
                EntityEklenenNakit.BankaKasaKaynak = entityBankaKasa.Id;

                _nakitService.Update(EntityEklenenNakit);

                return RedirectToAction("BankaKasa", "BankaKasa");
            }

            return View(n);

        }
        [HttpGet]
        public IActionResult NakitGuncelle(int? nakitid)
        {
            ViewBag.Sayfa = "NAKİT ÖDEME BİLGİSİNİ GÜNCELLEME";
            ViewBag.Sirket = _sirketService.GetAll(true);
            ViewBag.Cari = _cariHesapService.GetAll(null, true);
            ViewBag.Banka = _bankaHesapService.GetAll(true);

            if (nakitid == null)
            {
                return NotFound();
            }
            ENakit nakit = _nakitService.GetById((int)nakitid);

            if (nakit == null)
            {
                return NotFound();
            }
            return View(nakit);
        }
        [HttpPost]
        public IActionResult NakitGuncelle(ENakit n)
        {
            var entityNakit = _nakitService.GetById(n.Id);

            if (entityNakit == null)
            {
                return NotFound();
            }

            int? bankakasaid = entityNakit.BankaKasaKaynak;
            int? carikasaid = entityNakit.CariKasaKaynak;

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

            entityNakit.Tarih = n.Tarih;
            entityNakit.Aciklama = n.Aciklama;
            entityNakit.Tutar = n.Tutar;
            entityNakit.ImgUrl = n.ImgUrl;
            entityNakit.SonGuncelleme = System.DateTime.Now;
            entityNakit.CariHesapId = n.CariHesapId;
            entityNakit.SirketId = n.SirketId;
            entityNakit.BankaHesapId = n.BankaHesapId;

            _nakitService.Update(entityNakit);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Nakit(int? nakitid)
        {
            ViewBag.Sayfa = "NAKİT DETAYI";
            if (nakitid == null)
            {
                return NotFound();
            }

            ENakit nakit = _nakitService.GetById((int)nakitid);

            if (nakit == null)
            {
                return NotFound();
            }

            ViewBag.Sirket = _sirketService.GetAll();
            ViewBag.Cari = _cariHesapService.GetAll();
            ViewBag.Banka = _bankaHesapService.GetAll();

            return View(nakit);
        }
        [HttpGet]
        public IActionResult NakitSil(int? nakitid)
        {
            ViewBag.Sayfa = "NAKİT ÖDEMESİNİ SİL";


            if (nakitid == null)
            {
                return NotFound();
            }
            ENakit nakit = _nakitService.GetById((int)nakitid);

            if (nakit == null)
            {
                return NotFound();
            }
            return View(nakit);
        }
        [HttpPost]
        public IActionResult NakitSil(ENakit n)
        {
            var entityNakit = _nakitService.GetById(n.Id);
            if (entityNakit == null)
            {
                return NotFound();
            }

            entityNakit.SonGuncelleme = System.DateTime.Now;
            entityNakit.Durum = false;

            int? bankakasaid = entityNakit.BankaKasaKaynak;
            int? carikasaid = entityNakit.CariKasaKaynak;

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

            _nakitService.Update(entityNakit);
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

            return View(new ENakit());
        }
        [HttpPost]
        public async Task<IActionResult> NakitEklemeFromCari(ENakit n, IFormFile file)
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
                    var nakitName = string.Format($"{n.Aciklama}{"-"}{Guid.NewGuid()}{extension}");
                    n.ImgUrl = nakitName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\NakitResim", nakitName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                else { return View(n); }
            }
            else { return View(n); }
            #endregion
            _nakitService.Create(n);
            //CARİ KASA İÇİN ÇEK OLUŞTURULDU VE ÇEK KAYNAĞI İLE EKLENDİ
            String FirmaAdiForAciklama = _cariHesapService.GetById((int)n.CariHesapId).Ad;

            ECariKasa EntityCariKasa = new ECariKasa()
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
            EBankaKasa EntityBankaKasa = new EBankaKasa()
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
            var EntityEklenenNakit = _nakitService.GetById(n.Id);
            if (EntityEklenenNakit == null)
            {
                return NotFound();
            }
            EntityEklenenNakit.CariKasaKaynak = EntityCariKasa.Id;
            EntityEklenenNakit.BankaKasaKaynak = EntityBankaKasa.Id;

            _nakitService.Update(EntityEklenenNakit);

            return RedirectToAction("CariKasa", "CariKasa", new { carihesapid = n.CariHesapId });
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
    }
}
